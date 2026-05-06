using Microsoft.EntityFrameworkCore;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Infrastructure.Persistence;

namespace Slottet.Infrastructure.Repositories;

public class EfRiskRepository : IRiskRepository
{
    private readonly EFContext _context;

    public EfRiskRepository(EFContext context)
    {
        _context = context;
    }

    public async Task<Risk?> GetByIdAsync(int id)
    {
        return await _context.Risks.FindAsync(id);
    }

    public async Task UpdateAsync(Risk risk)
    {
        _context.Risks.Update(risk);
        await _context.SaveChangesAsync();
    }
}