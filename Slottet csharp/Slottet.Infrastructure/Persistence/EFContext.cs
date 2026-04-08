using System;
using Microsoft.EntityFrameworkCore;
using Slottet.Domain.Entities;

namespace Slottet.Infrastructure.Persistence;

public class EFContext : DbContext
{
    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {
    }
    // Exposes the entities to the Entity Framework engine
    // null! is used to suppress nullable warnings since EF will populate these properties at runtime
    public DbSet<Staff> Staffs { get; set; } = null!;
    public DbSet<Resident> Residents { get; set; } = null!;
}
