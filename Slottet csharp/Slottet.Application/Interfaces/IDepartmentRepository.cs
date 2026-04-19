using System;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IDepartmentRepository
{
    Task<Department> GetById(int id);
    Task<List<Department>> GetAll();
    Task Add(Department department);
    void Update(Department department);
    void Delete(Department department);

}


