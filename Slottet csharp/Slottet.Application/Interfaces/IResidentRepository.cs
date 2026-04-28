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

    // Fetch residents that are archived and have ArchivedAt date older than specified threshold date.
    Task<IEnumerable<Resident>> GetArchivedOlderThanAsync(DateTime thresholdDate, CancellationToken cancellationToken);

    // Permanently remove a collection of residents from database
    Task RemoveRangeAsync(IEnumerable<Resident> residents, CancellationToken cancellationToken);
}