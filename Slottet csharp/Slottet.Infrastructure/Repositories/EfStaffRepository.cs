using System;
using Slottet.Infrastructure.Persistence;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Slottet.Infrastructure.Repositories;

public class EfStaffRepository : IStaffRepository
{
    private readonly EFContext _context;

    public EfStaffRepository(EFContext context)
    {
        _context = context;
    }

    public async Task<Staff> Add(Staff staff)
    {
        try
        {
            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();
            return staff; 
        }
        // Catching Db exception and translating to general exception, which is thrown to API-layer, where its handled and turned into a HTTP response
        catch (DbUpdateException ex)
        {
            throw new Exception("Kunne ikke gemme personalet i databasen.", ex);
        }
    }
}
