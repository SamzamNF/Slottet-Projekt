using System;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IPhoneRepository
{
    Task<Phone> GetById(int id);
    Task<List<Phone>> GetAll();
    Task Add(Phone phone);
    void Update(Phone phone);
    void Delete(Phone phone);
}
