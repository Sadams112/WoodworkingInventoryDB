using Microsoft.EntityFrameworkCore;
using Program.cs;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure the database connection here.
        
        optionsBuilder.UseSqlite("Data Source=orders.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}

