using System;
using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IStaffRepository
{
    Task<Staff> Add(Staff staff);
}
