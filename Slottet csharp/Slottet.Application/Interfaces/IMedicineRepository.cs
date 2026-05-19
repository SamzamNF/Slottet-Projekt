using System.Collections.Generic;
using System.Threading.Tasks;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IMedicineRepository
{
    Task<Medicine?> GetByIdAsync(int id);
    Task<IEnumerable<Medicine>> GetByResidentIdAsync(int residentId);
    Task AddAsync(Medicine medicine);
    Task Update(Medicine medicine);
    Task Delete(Medicine medicine);
}