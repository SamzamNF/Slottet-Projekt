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

    public DbSet<Staff> Staffs { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;


    // Implementing IUnitOfWork, which saves changes as a transaction to the database
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

}
