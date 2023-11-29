using Microsoft.EntityFrameworkCore;
using Program.cs;
using SQLitePCL;


using Microsoft.EntityFrameworkCore;

public class OrderDbContext : DbContext
{
    private string dbPath;
    
    
    public DbSet<Order> Orders { get; set; }

    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder.UseSqlite($"Data Source={this.dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasKey(o => o.PO);

        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.PO); 
    }

    public OrderDbContext(string dbPath)
    {
        this.dbPath = dbPath;
    }
}


