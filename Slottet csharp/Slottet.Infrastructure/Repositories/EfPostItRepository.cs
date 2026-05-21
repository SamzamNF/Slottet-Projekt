using System.Data;
using Microsoft.EntityFrameworkCore;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Infrastructure.Persistence;

namespace Slottet.Infrastructure.Repositories;

public class EfPostItRepository  : IPostItRepository
{
    private readonly EFContext _context;

    public EfPostItRepository(EFContext context)
    {
        _context = context;
    }
    public async Task Create(PostIt postIt)
    {
        await _context.PostIts.AddAsync(postIt);
    }


    public async Task<List<PostIt>> GetAll()
    {
        return await _context.PostIts
            .Include(p => p.Risk)
            .Include(p => p.Medicines)
            .Include(p => p.PnTimes)
            .Include(p => p.Staff)
            .Include(p => p.Resident)
            .ToListAsync();
    }

    public async Task<List<PostIt>> GetByDate(DateTime date)
    {
        // Get all post-Its from the database where the date matches the desired date
        return await _context.PostIts
            .Where(p => p.Date.Date == date.Date)
            .Include(p => p.Risk)
            .Include(p => p.Medicines)
            .Include(p => p.PnTimes)
            .Include(p => p.Staff)
            .Include(p => p.Resident)
            .ToListAsync();
    }

    public async Task<PostIt> GetById(int id)
    {
        var postIt = await _context.PostIts
            .Include(p => p.Risk)
            .Include(p => p.Medicines)
            .Include(p => p.PnTimes)
            .Include(p => p.Staff)
            .Include(p => p.Resident)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (postIt == null)
            throw new KeyNotFoundException($"PostIt with ID {id} not found.");

        return postIt;
    }


    public void Update(PostIt postIt)
    {
        _context.PostIts.Update(postIt);
    }

    public void DeleteById(PostIt postIt)
    {
        _context.PostIts.Remove(postIt);
    }
}

