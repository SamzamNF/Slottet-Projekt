using System;
using System.Collections.Generic;
using System.Text;
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

    public async Task<PostIt> UpdatePostIt(PostIt postIt)
    {
        _context.PostIts.Update(postIt);
        await _context.SaveChangesAsync();
        return postIt;
    }
}

