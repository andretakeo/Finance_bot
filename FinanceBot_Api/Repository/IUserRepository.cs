using FinanceBot_Api.Models;
using FinanceBot_Api.Models.RequestModels;

namespace FinanceBot_Api.Repository;

public interface IUserRepository
{
    //GET METHODS
    public Task<List<User>> AllUsers();
    public Task<List<User>> ScoreBoard();
    public Task<List<User>> OneUser(string tag);
    
    
    //POST METHODS
    public Task<int> RegisterUser(RequestUser request);
    public Task<int> RegisterBuy(string tag, RequestStock request);
    
    
    //PUT METHODS
    public Task<int> UpdateUser(string tag, RequestUser request);
    public Task<double> Collect(string tag);
    
    
    //DELETE METHODS
    public Task<int> DeleteUser(string tag);
    public Task<int> RegisterSell(string tag, int id, RequestStock request);
}