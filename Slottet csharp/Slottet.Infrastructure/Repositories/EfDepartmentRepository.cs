using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Slottet.Infrastructure.Repositories;

public class EfDepartmentRepository : IDepartmentRepository
{
    private readonly EFContext _context;

    public EfDepartmentRepository(EFContext context)
    {
        _context = context;
    }

    public async Task Add(Department department)
    {
        await _context.Departments.AddAsync(department);
    }

    public void Delete(Department department)
    {
        _context.Departments.Remove(department);
    }

    public async Task<List<Department>> GetAll()
    {
        return await _context.Departments.ToListAsync();
    }

    public async Task<Department> GetById(int id)
    {
        return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id) ?? throw new KeyNotFoundException($"Fandt ikke en afdeling med ID {id}");
    }

    public void Update(Department department)
    {
        _context.Departments.Update(department);
    }
}
