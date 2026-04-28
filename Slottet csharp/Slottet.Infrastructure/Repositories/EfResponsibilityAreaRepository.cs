using System;
using Slottet.Domain.Entities;
using Slottet.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Slottet.Infrastructure.Persistence;


namespace Slottet.Infrastructure.Repositories;

public class EfResponsibilityAreaRepository : IResponsibilityAreaRepository
{
    private readonly EFContext _context;

    public EfResponsibilityAreaRepository(EFContext context)
    {
        _context = context;
    }

    public async Task Add(ResponsibilityArea responsibilityArea)
    {
        await _context.ResponsibilityAreas.AddAsync(responsibilityArea);
    }
    
    // Gets a ResponsibilityArea by its ID, including its associated Staff and Phone data
    public async Task<ResponsibilityArea?> GetById(int id)
    {
        return await _context.ResponsibilityAreas
            .Include(r => r.Staff)
            .Include(r => r.Phone)
            .FirstOrDefaultAsync(r => r.Id == id) ?? throw new KeyNotFoundException($"Fandt ikke et ansvarsområde med ID {id}");
    }

    // Gets all ResponsibilityAreas, including their associated Staff and Phone data
    public async Task<List<ResponsibilityArea>> GetAll()
    {
        return await _context.ResponsibilityAreas
            .Include(r => r.Staff)
            .Include(r => r.Phone)
            .ToListAsync() ?? throw new KeyNotFoundException("Ingen ansvarsområder fundet");
    }

    public void Update(ResponsibilityArea responsibilityArea)
    {
        _context.ResponsibilityAreas.Update(responsibilityArea);
    }

    public void Delete(ResponsibilityArea responsibilityArea)
    {
        _context.ResponsibilityAreas.Remove(responsibilityArea);
    }
}
