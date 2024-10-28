using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>

{
    public DbSet<Advertisement> Advertisements { get; set; } = default!;
    public DbSet<Rating> Ratings { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    
    public DbSet<Location> Locations { get; set; } = default!;
    
    public DbSet<PetCategory> PetCategories { get; set; } = default!;
    
    public DbSet<Price> Prices { get; set; } = default!;
    
    public DbSet<Service> Services { get; set; } = default!;
    
    public DbSet<ServicePetCategory> ServicePetCategories { get; set; } = default!;
    
    public DbSet<Status> Statuses { get; set; } = default!;

    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // disable cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // if (this.Database.ProviderName!.Contains("InMemory"))
        // {
        //     builder.Entity<Contest>()
        //         .OwnsOne(e => e.ContestName, builder => { builder.ToJson(); });
        // }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
        {
            foreach (var prop in entity
                         .Properties
                         .Where(x => x.Metadata.ClrType == typeof(DateTime)))
            {
                Console.WriteLine(prop);
                prop.CurrentValue = ((DateTime) prop.CurrentValue).ToUniversalTime();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}