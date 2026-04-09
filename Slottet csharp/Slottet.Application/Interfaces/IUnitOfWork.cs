using System;

namespace Slottet.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
