using System;
using Slottet.Application.Interfaces;
using Slottet.Infrastructure.Persistence;
using Slottet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Slottet.Infrastructure.Repositories;

public class EfRoleRepository : IRoleRepository
{
    private readonly EFContext _context;
    public EfRoleRepository(EFContext context)
    {
        _context = context;       
    }

    public async Task Add(Role role)
    {
        await _context.Roles.AddAsync(role);
    }

    public void Delete(Role role)
    {
        _context.Roles.Remove(role);
    }

    public async Task<List<Role>> GetAll()
    {
        return await _context.Roles.ToListAsync();
    }

    // Finds the first role in db with the given ID or throws an exception if null is returned, which is handled in the api controller
    public async Task<Role> GetById(int id)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id) ?? throw new KeyNotFoundException($"Fandt ikke en rolle med ID {id}");
    }

    public void Update(Role role)
    {
        _context.Roles.Update(role);
    }
}
