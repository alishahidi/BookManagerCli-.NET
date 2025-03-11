using BookManagerCLI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagerCLI.Data;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=books.db");
    }
}