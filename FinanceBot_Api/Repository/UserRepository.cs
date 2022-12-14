using FinanceBot_Api.Data;
using FinanceBot_Api.Models;
using FinanceBot_Api.Models.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace FinanceBot_Api.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _db;

    public UserRepository(DataContext db)
    {
        _db = db;
    }
    
    public async Task<List<User>> AllUsers()
    {
        return await _db.Users.Include(user => user!.Stocks).ToListAsync();
    }
    public async Task<List<User>> ScoreBoard()
    {
        return await _db.Users.OrderBy(x => x.Balance).ToListAsync();
    }
    public async Task<List<User>> OneUser(string tag)
    {
        var result =  await _db.Users.Include(user => user!.Stocks).Where(user=> user != null && user.UserTag == tag).ToListAsync();
        return result;
    }

    public async Task<int> RegisterUser(RequestUser request)
    {
        var result = await _db.Users.FirstOrDefaultAsync(x => x.UserTag == request.UserTag);

        if (result == null)
        {
            var newUser = new User {Id = 0, Username = request.Username, UserTag = request.UserTag, Stocks = null, Balance = 10000000};
            _db.Add(newUser);
            await _db.SaveChangesAsync();
            return 200;
        }
        else
        {
            return 500;
        }

    }

    public async Task<int> RegisterBuy(string tag ,RequestStock request)
    {
        var user = _db.Users.First(u => u.UserTag == tag);
        var newBalance = user.Balance - (request.Price);
        if (newBalance < 0 ) return 500;
        
        else{
            
            Console.WriteLine(user.Balance);
            Console.WriteLine(request.Price);
            Console.WriteLine(newBalance);

            user.Balance = newBalance;
            var newStock = new Stock { Id = 0, Amount = request.Amount, Coin = request.Coin, UserId = user.Id };
            _db.Add(newStock);
            await _db.SaveChangesAsync();
            return 200;  
        }

    }

    public async Task<int> UpdateUser(string tag, RequestUser request)
    {
        var user = _db.Users.First(u => u != null && u.UserTag == tag);
        if (user == null) return 500;
        user.Username = request.Username;
        user.UserTag = request.UserTag;
        await _db.SaveChangesAsync();
        return 200;

    }

    public async Task<double> Collect(string tag)
    {
        var user = _db.Users.First(u => u != null && u.UserTag == tag);
        if (user == null) return 500;
        
        var nextCollect = user.LastCollect.AddHours(3);
        
        if (nextCollect == DateTime.Now){
            user.Balance += 5000000;
            user.LastCollect = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return 200;
        }

        
        
        return (nextCollect - DateTime.Now).TotalSeconds ;
    }

    public async Task<int> DeleteUser(string tag)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u != null && u.UserTag == tag);
        if (user == null)
        {
            return 500;
        }
        else
        {
            _db.Remove(user);
            await _db.SaveChangesAsync();
            return 200;            
        }

    }

    public async Task<int> RegisterSell(string tag, int id, RequestStock request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u != null && u.UserTag == tag);
        var currentStock = await _db.Stocks.FindAsync(id);

        if (currentStock == null || user == null)
        {
            return 500;
        }
        else
        {
            Console.WriteLine(user.Balance + " + " + request.Price + " = " + (user.Balance + request.Price));
            user.Balance += request.Price;
            _db.Remove(currentStock);
            await _db.SaveChangesAsync();
            return 200;            
        }
        

        
    }
}