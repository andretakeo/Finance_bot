using System.Numerics;

namespace FinanceBot_Api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string UserTag { get; set; }
    public int Balance { get; set; }

    public DateTime LastCollect { get; set; }
    public List<Stock>? Stocks { get; set; }

}