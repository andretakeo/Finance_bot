using FinanceBot_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceBot_Api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    
}