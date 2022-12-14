using FinanceBot_Api.Models;
using FinanceBot_Api.Models.RequestModels;
using FinanceBot_Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FinanceBot_Api.Controllers;

[ApiController]
[Route("v1/users")]
public class UserController:  ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {
        _repository = repository ?? throw new  ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _repository.AllUsers();
    }
    
    
    [HttpGet("{tag}")]
    public async Task<List<User>> GetAllUsers(string tag)
    {
        return await _repository.OneUser(tag);
    }
    
    [HttpGet("scores")]
    public async Task<List<User?>> Leaderboard()
    {
        return await _repository.ScoreBoard();
    }




    [HttpPost]
    public async Task<int> PostUser(RequestUser request)
    {
        return await _repository.RegisterUser(request);
    }
    
    [HttpPost("{tag}/stock")]
    public async Task<int> PostBuy(string tag, RequestStock request)
    {
        return await _repository.RegisterBuy(tag, request);
    }
    

    [HttpPut("collect/{tag}")]
    public async Task<double> Auxilio(string tag)
    {
        return await _repository.Collect(tag);
    }
    
    [HttpDelete("{tag}")]
    public async Task<int> DeleteUser(string tag)
    {
        return await _repository.DeleteUser(tag);
    }
    
    [HttpDelete("{tag}/stocks/{id}")]
    public async Task<int> Sell(string tag, int id, RequestStock request)
    {
        return await _repository.RegisterSell(tag, id, request);
    }

}