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
        modelBuilder.Entity<Resident>().HasIndex(r => r.IsArchived);

        // Global query filter: Exclude archived residents from all queries
        modelBuilder.Entity<Resident>().HasQueryFilter(r => !r.IsArchived);
        
        // Global query filter: Exclude PostIts if the associated Resident is archived
        modelBuilder.Entity<PostIt>().HasQueryFilter(p => p.Resident == null || !p.Resident.IsArchived);
        
        // Global query filters for related entities to ensure they are also excluded when resident is archived
        modelBuilder.Entity<Medicine>().HasQueryFilter(m => m.PostIt == null || m.PostIt.Resident == null || !m.PostIt.Resident.IsArchived);
        modelBuilder.Entity<PnTime>().HasQueryFilter(pt => pt.PostIt == null || pt.PostIt.Resident == null || !pt.PostIt.Resident.IsArchived);
        modelBuilder.Entity<Risk>().HasQueryFilter(r => r.PostIt == null || r.PostIt.Resident == null || !r.PostIt.Resident.IsArchived);

        // Delete when DB is updated with these:
        //modelBuilder.Entity<Resident>().Ignore(r => r.IsArchived);
        //modelBuilder.Entity<Resident>().Ignore(r => r.ArchivedAt);

        // Define delete behavior for Staff-PostIt relationship to prevent cascading delete
        modelBuilder.Entity<PostIt>()
            .HasOne(p => p.Staff)
            .WithMany()
            .HasForeignKey(p => p.StaffId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        // Define 1-to-1 relationship between PostIt and Risk
        modelBuilder.Entity<PostIt>()
            .HasOne(p => p.Risk)
            .WithOne(r => r.PostIt) 
            .HasForeignKey<Risk>(r => r.PostItId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Risk>()
            .Property(r => r.PostItId)
            .IsRequired();  

        // Maps the ResponsibilityArea entity to the "ResponsibilityAreas" table in the database
        modelBuilder.Entity<ResponsibilityArea>().ToTable("Responsibility_areas");

        // Maps the PostIt entity to the "Post_its" table in the database
        modelBuilder.Entity<PostIt>().ToTable("Post_its");

        // Maps the PnTime entity to the "Pn_times" table in the database
        modelBuilder.Entity<PnTime>().ToTable("Pn_times");

        modelBuilder.Entity<PnTime>()
            .HasOne(pt => pt.PostIt)
            .WithMany(p => p.PnTimes)
            .HasForeignKey(pt => pt.PostItId)
            .OnDelete(DeleteBehavior.Cascade);  

        modelBuilder.Entity<Medicine>()
            .HasOne(m => m.PostIt)
            .WithMany(p => p.Medicines)
            .HasForeignKey(m => m.PostItId)
            .OnDelete(DeleteBehavior.Cascade);  




        // Define a trigger for auditing changes to the Role entity
        modelBuilder.Entity<Role>()
            .ToTable(tb => tb.HasTrigger("trg_Role_Audit"));
        modelBuilder.Entity<Department>()
            .ToTable(tb => tb.HasTrigger("trg_Department_Audit"));
        modelBuilder.Entity<Phone>()
            .ToTable(tb => tb.HasTrigger("trg_Phone_Audit"));
        modelBuilder.Entity<Resident>()
            .ToTable(tb => tb.HasTrigger("trg_Resident_Audit"));

        base.OnModelCreating(modelBuilder);
    }
}