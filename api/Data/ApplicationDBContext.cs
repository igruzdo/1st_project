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
    public DbSet<Portfolio> portfolios{ get; set; }
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new {p.AppUserId, p.StockId}));

        modelBuilder.Entity<Portfolio>()
            .HasOne(x => x.AppUser)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(u => u.AppUserId);

        modelBuilder.Entity<Portfolio>()
            .HasOne(x => x.Stock)
            .WithMany(u => u.Portfolios)
            .HasForeignKey(u => u.StockId);

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