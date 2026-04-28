using System;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IResponsibilityAreaRepository
{
    Task Add(ResponsibilityArea responsibilityArea);
    Task<ResponsibilityArea?> GetById(int id);
    Task<List<ResponsibilityArea>> GetAll();
    void Update(ResponsibilityArea responsibilityArea);
    void Delete(ResponsibilityArea responsibilityArea);
}
