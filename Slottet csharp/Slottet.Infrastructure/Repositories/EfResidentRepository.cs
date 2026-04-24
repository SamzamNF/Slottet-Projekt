using Microsoft.EntityFrameworkCore;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Infrastructure.Persistence;

namespace Slottet.Infrastructure.Repositories;

public class EfResidentRepository : IResidentRepository
{
    private readonly EFContext _context;

    public EfResidentRepository(EFContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Resident resident)
    {
        // Tells EF Core to start tracking this new entity
        await _context.Residents.AddAsync(resident);
    }
    public async Task<Resident?> GetByIdAsync(int id)
    {
        // Fetch single resident by ID
        return await _context.Residents.FirstOrDefaultAsync(r => r.Id == id);
    }

    // Fetch residents that are archived and have ArchivedAt date older than specified threshold date.
    public async Task<IEnumerable<Resident>> GetArchivedOlderThanAsync(DateTime thresholdDate, CancellationToken cancellationToken)
    {
        // Fetch residents that are archived and have an ArchivedAt date older than the threshold
        return await _context.Residents
            .Where(r => r.IsArchived && r.ArchivedAt != null && r.ArchivedAt < thresholdDate)
            .ToListAsync(cancellationToken);
    }

    // Permanently remove a collection of residents from the database
    public async Task RemoveRangeAsync(IEnumerable<Resident> residents, CancellationToken cancellationToken)
    {
        _context.Residents.RemoveRange(residents);
        await _context.SaveChangesAsync(cancellationToken);
    }
}