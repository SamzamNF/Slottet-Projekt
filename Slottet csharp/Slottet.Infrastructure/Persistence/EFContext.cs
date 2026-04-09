using System;
using Microsoft.EntityFrameworkCore;
using Slottet.Domain.Entities;

namespace Slottet.Infrastructure.Persistence;

public class EFContext : DbContext
{
    public EFContext(DbContextOptions<EFContext> options) : base(options)
    {
    }

    public DbSet<Staff> Staffs { get; set; } = null!;
    public DbSet<Medicine> Medicines { get; set; } = null!;
    public DbSet<PostIt> PostIts { get; set; } = null!;
    public DbSet<Resident> Residents { get; set; } = null!;
}
