using System;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IPnTimeRepository
{
    Task Add(PnTime pnTime);
    Task<PnTime?> GetById(int id);
    Task<List<PnTime>> GetAll();
    void Update(PnTime pnTime);
    void Delete(PnTime pnTime);
}
