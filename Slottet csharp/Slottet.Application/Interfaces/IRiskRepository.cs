using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IRiskRepository
{
    Task<Risk?> GetByIdAsync(int id);
    Task UpdateAsync(Risk risk);
}