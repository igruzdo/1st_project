using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    // Определите DbSet для каждой сущности
    public DbSet<Stock> Stocks { get; set; }  
    public DbSet<Comment> Comments { get; set; }
}