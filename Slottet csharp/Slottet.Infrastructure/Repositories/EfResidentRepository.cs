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
}