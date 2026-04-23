using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IResidentRepository
{
    // Prepares the resident to be added to the database
    Task AddAsync(Resident resident);
    // Retrieve a resident by ID
    Task<Resident?> GetByIdAsync(int id);

    // Add this method to fix CS1061
    Task<IEnumerable<Resident>> GetArchivedOlderThanAsync(DateTime thresholdDate, CancellationToken cancellationToken);

    // Add this method to fix usage in ResidentRetentionService
    Task RemoveRangeAsync(IEnumerable<Resident> residents, CancellationToken cancellationToken);
}