using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    // Определите DbSet для каждой сущности
    public DbSet<Stock> Stocks { get; set; }  
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        List<IdentityRole> identityRoles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
            },
        };
        modelBuilder.Entity<IdentityRole>().HasData(identityRoles); 
    }
}