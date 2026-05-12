using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class MedicineService
{
    private readonly IMedicineRepository _medicineRepo;
    private readonly IUnitOfWork _unitOfWork;

    // Inject medicine repository and unit of work via constructor
    public MedicineService(IMedicineRepository medicineRepo, IUnitOfWork unitOfWork)
    {
        _medicineRepo = medicineRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MedicineDto>> GetByResidentAsync(int residentId)
    {
        var medicines = await _medicineRepo.GetByResidentIdAsync(residentId);

        return medicines.Select(m => new MedicineDto
        {
            Id = m.Id,
            ResidentId = m.ResidentId,
            StaffId = m.StaffId,
            Description = m.Description,
            TimeStamp = m.TimeStamp,
            CreatedAt = m.CreatedAt,
            IsFromToday = m.IsFromToday,
            CanBeEditedOrDeleted = m.CanBeEditedOrDeleted()
        });
    }

    public async Task<MedicineDto> CreateAsync(MedicineDto dto)
    {
        var medicine = Medicine.Create(dto.ResidentId, dto.StaffId, dto.Description, dto.TimeStamp);

        await _medicineRepo.AddAsync(medicine);

        // Save using UnitOfWork implementation
        await _unitOfWork.SaveChangesAsync();

        // Map assigned values back to DTO
        dto.Id = medicine.Id;
        dto.CreatedAt = medicine.CreatedAt;
        dto.IsFromToday = medicine.IsFromToday;
        dto.CanBeEditedOrDeleted = medicine.CanBeEditedOrDeleted();

        return dto;
    }

    public async Task UpdateAsync(int id, string newDescription, bool isAdmin)
    {
        var medicine = await _medicineRepo.GetByIdAsync(id);

        if (medicine == null)
            throw new KeyNotFoundException("Medicin dokumentation blev ikke fundet");

        // Pass  role check to domain entity
        medicine.Update(newDescription, isAdmin);

        await _medicineRepo.Update(medicine);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, bool isAdmin)
    {
        var medicine = await _medicineRepo.GetByIdAsync(id);
        if (medicine == null)
            throw new KeyNotFoundException("Medicin dokumentation blev ikke fundet");

        // Validate domain rule mixed with role check
        if (!isAdmin && !medicine.CanBeEditedOrDeleted())
            throw new InvalidOperationException("Tidsgrænsen for at redigere/slette er overskredet");

        await _medicineRepo.Delete(medicine);
        await _unitOfWork.SaveChangesAsync();
    }
}