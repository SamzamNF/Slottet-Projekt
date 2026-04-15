using System;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetById(int id);
    Task<List<Role>> GetAll();
    Task Add(Role role);
    void Update(Role role);
    void Delete(Role role);

}
