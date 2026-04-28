using Microsoft.EntityFrameworkCore;
using Slottet.Domain.Entities;
using Slottet.Infrastructure.Persistence;
using Slottet.Application.Interfaces;

namespace Slottet.Infrastructure.Repositories;

public class EfPostItRepository  : IPostItRepository
{
    private readonly EFContext _context;

    public EfPostItRepository(EFContext context)
    {
        _context = context;
    }

    public async Task<List<PostIt>> GetAll()
    {
        return await _context.PostIts.ToListAsync();
    }

    public async Task<List<PostIt>> GetByDate(DateTime date)
    {
        // Get all post-Its from the database where the date matches the desired date. Use Date.Date to ignore the time component.
        return await _context.PostIts
            .Where(p => p.Date.Date == date.Date)
            .ToListAsync();
    }

    public async Task<PostIt> UpdatePostIt(PostIt postIt)
    {
        _context.PostIts.Update(postIt);
        return postIt;
    }

    public async Task<List<PostIt>> GetAllAsync()
    {
        return await _context.PostIts.ToListAsync();
    }
}

