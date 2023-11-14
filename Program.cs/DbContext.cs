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

        // Call SetProvider to configure SQLitePCL.raw
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}


