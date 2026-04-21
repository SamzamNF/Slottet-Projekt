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

    // Gets a Staff member by their ID, including their associated Department and Role data
    public async Task<Staff?> GetById(int id)
    {
        return await _context.Staffs
            .Include(s => s.Department)
            .Include(s => s.Role)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    // Gets all Staff members, including their associated Department and Role data
    public async Task<List<Staff>> GetAll()
    {
        return await _context.Staffs
            .Include(s => s.Department)
            .Include(s => s.Role)
            .ToListAsync();
    }

    public void Update(Staff staff)
    {
        _context.Staffs.Update(staff);
    }

    public void Delete(Staff staff)
    {
        _context.Staffs.Remove(staff);
    }

}
