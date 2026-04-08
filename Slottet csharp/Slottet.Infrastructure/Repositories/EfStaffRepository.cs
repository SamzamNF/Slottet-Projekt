using System;
using Slottet.Infrastructure.Persistence;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Slottet.Infrastructure.Repositories;

public class EfStaffRepository : IStaffRepository
{
    private readonly EFContext _context;

    public EfStaffRepository(EFContext context)
    {
        _context = context;
    }

    public async Task<Staff> Add(Staff staff)
    {
        await _context.Staffs.AddAsync(staff);
        return staff; 
    }
}
