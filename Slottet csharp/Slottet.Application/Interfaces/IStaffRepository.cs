using System;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IStaffRepository
{
    Task Add(Staff staff);
    Task<Staff?> GetById(int id);
    Task<List<Staff>> GetAll();
    void Update(Staff staff);
    void Delete(Staff staff);
}
