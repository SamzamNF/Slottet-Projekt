using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class DepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentService(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
    {
        _departmentRepository = departmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<DepartmentDTO>> GetAllDepartments()
    {
        List<Department> departments = await _departmentRepository.GetAll();

        if (departments == null || departments.Count == 0)
            throw new KeyNotFoundException("Ingen afdelinger fundet");

        // Takes each department in the list, and maps it to the DTO, then returns it to the API
        return departments.Select(d => MapToDto(d)).ToList();
    }

    public async Task<DepartmentDTO> GetById(int id)
    {
        Department department = await _departmentRepository.GetById(id);

        if (department == null)
            throw new KeyNotFoundException($"Afdeling med ID {id} blev ikke fundet");

        // Maps the department to a DTO and returns it API
        return MapToDto(department);
    }

    public async Task<DepartmentDTO> Add(DepartmentDTO departmentDto)
    {
        Department department = Department.Create(departmentDto.DepartmentName);

        await _departmentRepository.Add(department);

        // Checks if the department was added to the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den oprettede afdeling i databasen.");

        // Gets the created department to get the newyly generated ID by the DB, and returns it to the API as a DTO
        Department created = await _departmentRepository.GetById(department.Id);
        if (created == null)
            throw new KeyNotFoundException("Afdeling kunne ikke findes efter oprettelsen");
        
        return MapToDto(created);
    }

    public async Task Update(DepartmentDTO departmentDto)
    {
        Department department = await _departmentRepository.GetById(departmentDto.Id);
        if (department == null)
            throw new KeyNotFoundException($"Afdeling med ID {departmentDto.Id} blev ikke fundet");

        // Updates the department with the new values from the DTO
        department.Update(departmentDto.DepartmentName);

        _departmentRepository.Update(department);

        // Checks if the department was updated in the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke opdatere afdelingen i databasen.");

    }

    public async Task Delete(int id)
    {
        Department department = await _departmentRepository.GetById(id);
        if (department == null)
            throw new KeyNotFoundException($"Afdeling med ID {id} blev ikke fundet");

        _departmentRepository.Delete(department);

        // Checks if the department was deleted in the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke slette afdelingen i databasen.");
    }

    private DepartmentDTO MapToDto(Department department)
    {
        return new DepartmentDTO
        {
            Id = department.Id,
            DepartmentName = department.DepartmentName ?? string.Empty
        };
    }
}
