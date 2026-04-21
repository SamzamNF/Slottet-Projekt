using System;
using Microsoft.EntityFrameworkCore;
using Slottet.Domain.Entities;
using Slottet.Application.Interfaces;

namespace Slottet.Infrastructure.Persistence;

public class EFContext : DbContext, IUnitOfWork
{
    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {
    }
    // Exposes the entities to the Entity Framework engine
    // null! is used to suppress nullable warnings since EF will populate these properties at runtime
    public DbSet<Staff> Staffs { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Medicine> Medicines { get; set; } = null!;
    public DbSet<PostIt> PostIts { get; set; } = null!;
    public DbSet<Resident> Residents { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;

    // Implementing IUnitOfWork, which saves changes as a transaction to the database
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

}
