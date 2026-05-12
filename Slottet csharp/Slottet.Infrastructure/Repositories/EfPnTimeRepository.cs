using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Slottet.Infrastructure.Persistence;

namespace Slottet.Infrastructure.Repositories;

public class EfPnTimeRepository : IPnTimeRepository
{
    private readonly EFContext _context;

    public EfPnTimeRepository(EFContext context)
    {
        _context = context;
    }

    public async Task Add(PnTime pnTime)
    {
        await _context.PnTimes.AddAsync(pnTime);
    }

    public void Delete(PnTime pnTime)
    {
        _context.PnTimes.Remove(pnTime);
    }

    public async Task<List<PnTime>> GetAll()
    {
        return await _context.PnTimes.ToListAsync();
    }

    public async Task<PnTime?> GetById(int id)
    {
        return await _context.PnTimes.FindAsync(id) ?? throw new KeyNotFoundException($"Pn Tid med ID: {id} blev ikke fundet");
    }

    public void Update(PnTime pnTime)
    {
        _context.PnTimes.Update(pnTime);
    }
}
