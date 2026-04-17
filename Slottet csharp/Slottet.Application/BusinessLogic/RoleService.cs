using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;

    }

    public async Task<List<RoleDTO>> GetAllRoles()
    {
        List<Role> roles = await _roleRepository.GetAll();
        if (roles == null || roles.Count == 0)
        {
            throw new KeyNotFoundException("Ingen roller fundet");
        }

        // Takes each role in the list and transforms it to a DTO and returns it to the API
        return roles.Select(role => MapToDto(role)).ToList();
    }

    public async Task<RoleDTO> GetById(int id)
    {
        Role role = await _roleRepository.GetById(id);
        if (role == null)
            throw new InvalidOperationException($"Rolle med ID {id} blev ikke fundet");

        // Maps the role to a DTO and returns it API
        return MapToDto(role);
    }

    public async Task<RoleDTO> Add(RoleDTO roleDto)
    {
        Role role = Role.Create(roleDto.Name);

        await _roleRepository.Add(role);

        // Checks if the role was added to the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke gemme den oprettede rolle i databasen.");

        // Fetches the role to get the generated ID by the DB
        Role created = await _roleRepository.GetById(role.Id);
        if (created == null)
            throw new InvalidOperationException("Rolle kunne ikke findes efter oprettelsen");
        
        // Maps the created role to a DTO and returns it API
        return MapToDto(created);        
    }

    public async Task Update(RoleDTO roleDto)
    {
        Role? role = await _roleRepository.GetById(roleDto.Id);
        if (role == null)
            throw new InvalidOperationException($"Rolle med ID {roleDto.Id} blev ikke fundet");
        
        // Updates the old role object with the new values from the DTO
        role.Update(roleDto.Name);

        _roleRepository.Update(role);

        // Checks if the role was updated in the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke opdatere rollen i databasen.");
    }

    public async Task Delete(int id)
    {
        Role? role = await _roleRepository.GetById(id);

        if (role == null)
            throw new InvalidOperationException($"Rolle med ID {id} blev ikke fundet");

        // Deletes the role from the database
        _roleRepository.Delete(role);

        // Checks if the role was deleted from the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke slette rollen i databasen.");
    }

    private RoleDTO MapToDto(Role role)
    {
        return new RoleDTO
        {
            Id = role.Id,
            Name = role.RoleName
        };
    }

}
