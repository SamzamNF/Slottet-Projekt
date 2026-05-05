using System;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class PhoneService
{
    private readonly IPhoneRepository _phoneRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PhoneService(IPhoneRepository phoneRepository, IUnitOfWork unitOfWork)
    {
        _phoneRepository = phoneRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PhoneDTO>> GetAllPhones()
    {
        List<Phone> phones = await _phoneRepository.GetAll();

        if (phones == null || phones.Count == 0)
            throw new KeyNotFoundException("Ingen telefoner fundet.");

        return phones.Select(phone => MapToDTO(phone)).ToList();
    }

    public async Task<PhoneDTO> GetById(int id)
    {
        Phone phone = await _phoneRepository.GetById(id);

        if (phone == null)
            throw new KeyNotFoundException($"Telefon med ID {id} ikke fundet.");

        return MapToDTO(phone);
    }

    public async Task<PhoneDTO> Add(PhoneDTO phoneDTO)
    {
        Phone phone = Phone.Create(phoneDTO.PhoneNumber);

        await _phoneRepository.Add(phone);

        // Checks if the phone was added to the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Telefonen kunne ikke oprettes i databasen.");

        Phone created = await _phoneRepository.GetById(phone.Id);
        if (created == null)
            throw new KeyNotFoundException("Telefonen kunne ikke hentes efter oprettelse.");

        return MapToDTO(created);
    }

    public async Task Update(PhoneDTO phoneDTO)
    {
        Phone? phone = await _phoneRepository.GetById(phoneDTO.Id);
        if (phone == null)
            throw new KeyNotFoundException($"Telefon med ID {phoneDTO.Id} ikke fundet.");

        // Updates the old phone with the new values from the DTO
        phone.Update(phoneDTO.PhoneNumber);

        // Saves to the database
        _phoneRepository.Update(phone);

        // Check if the phone was updated in the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Telefonen kunne ikke opdateres i databasen.");
    }

    public async Task Delete(int id)
    {
        Phone? phone = await _phoneRepository.GetById(id);
        if (phone == null)
            throw new KeyNotFoundException($"Telefon med ID {id} ikke fundet.");

        _phoneRepository.Delete(phone);

        // Check if the phone was deleted from the database and throws an exception if not
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Telefonen kunne ikke slettes fra databasen.");
    }
    
    // Not really needed, but added in case we want to add more complex logic in the future
    private PhoneDTO MapToDTO(Phone phone)
    {
        return new PhoneDTO
        {
            Id = phone.Id,
            PhoneNumber = phone.PhoneNumber
        };
    }
}
