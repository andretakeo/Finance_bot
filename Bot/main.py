import discord
import requests
import json

from discord.app_commands import CommandNotFound
from discord.ext import commands

import pandas as pd
import matplotlib.pyplot as plt
import datetime

intents = discord.Intents.default()
intents.message_content = True

url = "http://localhost:5000/v1/users/"

def findUser(tag):
    r = requests.get(f"{url}{str(tag)}")
    r = json.loads(r.text)
    if r == []:
        return []
    else:
        return r[0]
def checkPrice(coin= None, currency="BRL"):
    api_url = f"https://www.binance.com/api/v3/ticker/price?symbol="
    r = requests.get(api_url+str(coin)+currency)
    r = json.loads(r.text)
    return float(r["price"])
class Person:
    def __init__(self, name, tag):
        self.name = name
        self.tag = tag

    def payload(self):
        payload = {
            "username": self.name,
            "userTag": self.tag
        }
        return payload

    def exist(self):
        r = requests.get(f"{url}{str(self.tag)}")
        r = json.loads(r.text)
        if r == []:
            return False
        else:
            return True
    def register(self):
        create = requests.post(f"{url}", data=json.dumps(self.payload()), headers={'content-type': 'application/json'})
        if create.text == "200":
            return f"Usuário {self.name} cadastrado."
        else:
            return f"Usuário {self.name} já existe."


    def collect(self):
        collecting = requests.put(f"http://localhost:5000/v1/users/collect/{self.tag}")
        collecting = json.loads(collecting.text)
        if collecting == 200:
            return "Foi adicionado R$50000.00 na sua conta via resgate do auxílio Brasil."
        else:

            hours = int(collecting / 3600)
            mins = int((collecting - (hours*3600))/60)

            return f"Calma aí {self.name}, faltam ainda {hours}h {mins}min. para você resgatar o auxílio Brasil, por hora, faz o L!"

    def stocks(self):
        user = findUser(self.tag)
        if user != []:
            msg = f"```Username: {self.name}#{self.tag}\nBalance: R${float(user['balance']/100)}\n\n"
            i = 1
            for stock in user["stocks"]:
                msg += f"""{i}. {stock['amount']/100} {stock['coin']}\n"""
                i += 1

            msg += "```"
            return msg
        else:
            return "Erro."

    def buy(self, stock_payload):
        create = requests.post(f"{url}{self.tag}/stock", data=json.dumps(stock_payload), headers={'content-type': 'application/json'})
        if create.text == "200":
            return "Comprado."
        else:
            return "Erro."

    def sell(self, id):
        user = findUser(self.tag)
        if user != []:
            stock = user["stocks"][int(id) - 1]
            price = int(checkPrice(stock["coin"]) * (stock["amount"]/100))
            one_stock = Stock(stock["amount"], stock["coin"], price)
            stock_payload = one_stock.payload()
            print(stock_payload)
            create = requests.delete(f"{url}{self.tag}/stocks/{int(stock['id'])}", data=json.dumps(stock_payload), headers={'content-type': 'application/json'})
            if create.text == "200":
                return "Vendido."
            else:
                return "Erro."
        else:
            return "Erro."
class Stock:
    def __init__(self, amount, coin, price):
        self.amount = float(amount)
        self.coin = str(coin).upper().strip()
        self.price = float(price)
    def payload(self):
        payload = {
            "coin": str(self.coin),
            "amount": int(self.amount * 100),
            "price": int(self.price * 100)
        }
        return payload



bot = commands.Bot(command_prefix="$", intents=intents)

# client = discord.Client(intents=intents)


@bot.event
async def on_ready():
    print('We have logged in as {0.user}'.format(bot))

@bot.event
async def on_command_error(ctx, error):
    if isinstance(error, CommandNotFound):
        await ctx.send("Comando não reconhecido.")



@bot.command()
async def register(ctx):
    new_user = Person(ctx.author.name, ctx.author.discriminator)
    return await ctx.send(new_user.register())

@bot.command()
async def crypto(ctx, amount = 1, coin = "btc"):
    amount = float(amount)
    price = checkPrice(str(coin).upper().strip())
    return await ctx.send(f"```{amount} {str(coin).upper().strip()} = R${float(price * amount)}```")

@bot.command()
async def collect(ctx):
    user = Person(ctx.author.name, ctx.author.discriminator)
    return await ctx.send(user.collect())

@bot.command()
async def buy_crypto(ctx, amount, coin):
    user = Person(ctx.author.name, ctx.author.discriminator)
    price = ((checkPrice(str(coin).upper().strip())) * (float(amount)*100))/100
    new_stock = Stock(amount, str(coin).upper().strip(), price)
    print(new_stock.payload())
    return await ctx.send(user.buy(new_stock.payload()))

@bot.command()
async def sell_crypto(ctx, id):
    user = Person(ctx.author.name, ctx.author.discriminator)
    if user != []:

        await ctx.send(user.sell(id))
        return 200

@bot.command()
async def wallet(ctx):
    user = Person(ctx.author.name, ctx.author.discriminator)
    return await ctx.send(user.stocks())


with open('Key.json') as f:
    BOTKEY = json.loads(f.read())
bot.run(BOTKEY["BOTKEY"])

