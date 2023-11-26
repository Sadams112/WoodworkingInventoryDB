using Microsoft.EntityFrameworkCore;
using Program.cs;
using SQLitePCL;


using Microsoft.EntityFrameworkCore;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
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


