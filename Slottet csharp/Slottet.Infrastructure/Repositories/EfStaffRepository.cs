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

    public async Task Add(Staff staff)
    {
        await _context.Staffs.AddAsync(staff);
    }

    public async Task<Staff?> GetById(int id)
    {
        return await _context.Staffs
            .Include(s => s.Department)
            .Include(s => s.Role)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Staff>> GetAll()
    {
        return await _context.Staffs
            .Include(s => s.Department)
            .Include(s => s.Role)
            .ToListAsync();
    }
}
