using System;
using Slottet.Application.Interfaces;
using Slottet.Infrastructure.Persistence;
using Slottet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Slottet.Infrastructure.Repositories;

public class EfPhoneRepository : IPhoneRepository
{
    private readonly EFContext _context;

    public EfPhoneRepository(EFContext context)
    {
        _context = context;
    }

    public async Task Add(Phone phone)
    {
        await _context.Phones.AddAsync(phone);
    }

    public void Delete(Phone phone)
    {
        _context.Phones.Remove(phone);
    }

    public async Task<List<Phone>> GetAll()
    {
        return await _context.Phones.ToListAsync();
    }

    public async Task<Phone> GetById(int id)
    {
        return await _context.Phones.FindAsync(id) ?? throw new KeyNotFoundException($"Phone with ID {id} not found.");
    }

    public void Update(Phone phone)
    {
        _context.Phones.Update(phone);
    }
}
