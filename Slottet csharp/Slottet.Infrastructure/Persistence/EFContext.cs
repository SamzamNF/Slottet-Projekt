using System;
using Microsoft.EntityFrameworkCore;
using Slottet.Domain.Entities;
using Slottet.Application.Interfaces;

namespace Slottet.Infrastructure.Persistence;

public class EFContext : DbContext, IUnitOfWork
{
    public EFContext(DbContextOptions<EFContext> options) : base(options) {}
    
    // Exposes the entities to the Entity Framework engine
    // null! is used to suppress nullable warnings since EF will populate these properties at runtime
    public DbSet<Staff> Staffs { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Medicine> Medicines { get; set; } = null!;
    public DbSet<PostIt> PostIts { get; set; } = null!;
    public DbSet<Resident> Residents { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<ResponsibilityArea> ResponsibilityAreas { get; set; } = null!;
    public DbSet<Phone> Phones { get; set; } = null!;
    public DbSet<PnTime> PnTimes { get; set; } = null!;
    public DbSet<Risk> Risks { get; set; }

    // Implementing IUnitOfWork, which saves changes as a transaction to the database
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    // Configures the model and database schema
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Performance optimization: Index for filtered queries
        // modelBuilder.Entity<Resident>().HasIndex(r => r.IsArchived);

        // Global query filter: Exclude archived residents from all queries
        // modelBuilder.Entity<Resident>().HasQueryFilter(r => !r.IsArchived);

        // Delete when DB is updated with these:
        modelBuilder.Entity<Resident>().Ignore(r => r.IsArchived);
        modelBuilder.Entity<Resident>().Ignore(r => r.ArchivedAt);

        // Define 1-to-1 relationship between PostIt and Risk
        modelBuilder.Entity<PostIt>()
            .HasOne<Risk>()
            .WithOne()
            .HasForeignKey<PostIt>(r => r.Id)
            .OnDelete(DeleteBehavior.Cascade); // Risk is deleted with PostIt

        // Maps the ResponsibilityArea entity to the "ResponsibilityAreas" table in the database
        modelBuilder.Entity<ResponsibilityArea>()
        .ToTable("Responsibility_areas");

        base.OnModelCreating(modelBuilder);
    }

}
