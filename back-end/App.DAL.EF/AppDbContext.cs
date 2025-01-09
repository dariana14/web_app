using App.Domain;
using App.Domain.Identity;
using App.Resources.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Grade = App.DAL.DTO.Grade;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>

{
    public DbSet<Subject> Subjects { get; set; } = default!;

    public DbSet<StudentSubject> StudentSubjects { get; set; } = default!;
    
    public DbSet<Domain.Grade> Grades { get; set; } = default!;


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
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
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