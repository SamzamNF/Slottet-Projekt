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
    public async Task CreatePostIt(PostIt postIt)
    {
        await _context.PostIts.AddAsync(postIt);
    }


    public async Task<List<PostIt>> GetAllPostIts()
    {
        return await _context.PostIts.ToListAsync();
    }

    public async Task<List<PostIt>> GetPostItByDate(DateTime date)
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

    public async Task DeletePostItById(int id)
    {
        var postIt = await _context.PostIts.FindAsync(id);
        if (postIt != null)
        {
            _context.PostIts.Remove(postIt);
        }
    }
}

