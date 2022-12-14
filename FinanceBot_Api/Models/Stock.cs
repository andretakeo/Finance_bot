namespace FinanceBot_Api.Models;

public class Stock
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public string Coin { get; set; }
    public int Amount { get; set; }
}