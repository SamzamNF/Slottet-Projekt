using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Infrastructure.Persistence;

namespace Slottet.Infrastructure.Repositories;

public class EfMedicineRepository : IMedicineRepository
{
    private readonly EFContext _context;

    public EfMedicineRepository(EFContext context)
    {
        _context = context;
    }

    public async Task<Medicine?> GetByIdAsync(int id)
    {
        return await _context.Medicines.FindAsync(id);
    }

    public async Task<IEnumerable<Medicine>> GetByResidentIdAsync(int residentId)
    {
        // Fetch history for a specific resident, newest first
        return await _context.Medicines
            .Where(m => m.ResidentId == residentId)
            .OrderByDescending(m => m.TimeStamp)
            .ToListAsync();
    }

    public async Task AddAsync(Medicine medicine)
    {
        await _context.Medicines.AddAsync(medicine);
    }

    public Task UpdateAsync(Medicine medicine)
    {
        _context.Medicines.Update(medicine);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Medicine medicine)
    {
        _context.Medicines.Remove(medicine);
        return Task.CompletedTask;
    }
}