# Finance_bot
Discord finance bot project.

## Bot

Create a file inside the "Bot" folder named "Key.json" and add a JSON:

{
  "BOTKEY": "YOUR_KEY"
}

After this, the bot should be working fine.


## API

After cloning the repository, make sure you have the .NET 7 SDK and PostgeSQL installed.

Inside of "FinanceBot_Api" folder, create a .json file called "appsettings.json" with:

{ "ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;user id=USER;password=PASSWORD;Database=FinanceBotDB;"
},
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

Then run "dotnet restore", this should install all dependencies to make it work.

Don't forget to install the dotnet-ef, run:

dotnet tool install --global dotnet-ef

With this, create a migration called "FinanceBotDB" and run.


