using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IResidentRepository
{
    // Prepares the resident to be added to the database
    Task AddAsync(Resident resident);
}