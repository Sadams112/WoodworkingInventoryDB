using Microsoft.EntityFrameworkCore;
using Program.cs;
using SQLitePCL;


using Microsoft.EntityFrameworkCore;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure the database connection here.
        // For example, using SQLite with a file-based database:
        optionsBuilder.UseSqlite("Data Source=orders.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasKey(o => o.PO);

        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.PO); 
    }
}


