using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class PostItService
{
    private readonly IPostItRepository _postItRepo;
    private readonly IUnitOfWork _unitOfWork;

    public PostItService(IPostItRepository postItRepo, IUnitOfWork unitOfWork)
    {
        _postItRepo = postItRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<PostItDTO> Create(PostItDTO postItDTO)
    {
        var postIt = PostIt.Create(
            postItDTO.Date,
            postItDTO.Payment,
            postItDTO.ShoppingDay,
            postItDTO.Mood,
            postItDTO.Status,
            postItDTO.Events,
            postItDTO.RelativesContact,
            postItDTO.StaffId,
            postItDTO.ResidentId,
            postItDTO.RiskAssessment
        );
        
        // If there are any PnTimes included in the DTO, add them to the PostIt, otherwise skip
        if (postItDTO.PnTimes != null && postItDTO.PnTimes.Count > 0)
        {
            foreach (var pnTimeDTO in postItDTO.PnTimes)
            {
                postIt.AddPnTime(PnTime.Create(pnTimeDTO.Time, pnTimeDTO.Description));
            }
        }

        // Requires Resident and Staff to be included, medicine is not required to create a PostIt, will just skip
        // if not included
        if (postItDTO.Medicines != null && postItDTO.Medicines.Count > 0)
        {
            if (postItDTO.ResidentId == 0 || postItDTO.StaffId == 0)
                throw new ArgumentException("Medicin kan ikke oprettes uden tilknytning til både beboer og medarbejder.");

            foreach (var medicineDTO in postItDTO.Medicines)
            {
                postIt.AddMedicine(Medicine.Create(
                medicineDTO.Description, 
                medicineDTO.TimeStamp
                ));
            }   
        }

        await _postItRepo.Create(postIt);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke oprette PostIt.");

        return MapToDTO(postIt);
    }

    public async Task<PostItDTO> Update(PostItDTO postItDTO)
    {
        var postIt = await _postItRepo.GetById(postItDTO.Id);
        if (postIt == null)
            throw new KeyNotFoundException($"Post-It med ID {postItDTO.Id} ikke fundet.");
        
        postIt.Update(
            postItDTO.Date,
            postItDTO.Payment,
            postItDTO.ShoppingDay,
            postItDTO.Mood,
            postItDTO.Status,
            postItDTO.Events,
            postItDTO.RelativesContact,
            postItDTO.StaffId,
            postItDTO.RiskAssessment
        );


        // If there are any PnTimes included in the DTO, update them to the PostIt, otherwise skip
        if (postItDTO.PnTimes != null)
        {
            // Check for new PnTimes, which will have an Id of 0, and add them to the PostIt
            var newPnTimes = postItDTO.PnTimes.Where(p => p.Id == 0);
            foreach (var pnTimeDTO in newPnTimes)
            {
                postIt.AddPnTime(PnTime.Create(pnTimeDTO.Time, pnTimeDTO.Description));
            }
            
            var pnTuples = postItDTO.PnTimes
                .Where(p => p.Id > 0) // Only include existing PnTimes that need to be updated, new ones are already added above
                .Select(p => (p.Id, p.Time, p.Description))
                .ToList();

            postIt.UpdatePnTimes(pnTuples);
        }

        if (postItDTO.Medicines != null)
        {
            // Check for new Medicines, which will have an Id of 0, and add them to the PostIt
            var newMedicines = postItDTO.Medicines.Where(m => m.Id == 0);
            foreach (var medicineDTO in newMedicines)
            {
                if (postItDTO.ResidentId == 0 || postItDTO.StaffId == 0)
                    throw new ArgumentException("Medicin kan ikke oprettes uden tilknytning til både beboer og medarbejder.");
                    
                postIt.AddMedicine(Medicine.Create(
                    medicineDTO.Description, 
                    medicineDTO.TimeStamp
                ));
            }
            
            var medicineTuples = postItDTO.Medicines
                .Where(m => m.Id > 0) // Only include existing Medicines that need to be updated, new ones are already added above
                .Select(m => (m.Id, m.Description))
                .ToList();

            postIt.UpdateMedicines(medicineTuples);
        }

        _postItRepo.Update(postIt);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke opdatere PostIt.");

        return MapToDTO(postIt);
    }

    public async Task<List<PostItDTO>> GetHistory(DateTime date)
    {
        // Call repository
        var dateHistory = await _postItRepo.GetByDate(date);
        // Return list of DTOs based on list of domain objects
        return dateHistory.Select(postIt => MapToDTO(postIt)).ToList();
    }

    public async Task<List<PostItDTO>> GetAllHistory()
    {
        List<PostIt> postIts = await _postItRepo.GetAll();
        return postIts.Select(postIt => MapToDTO(postIt)).ToList();
    }

    public async Task<PostItDTO> GetById(int id)
    {
        var postIt = await _postItRepo.GetById(id);
        if (postIt == null)
            throw new InvalidOperationException($"Post-It med ID {id} ikke fundet.");

        return MapToDTO(postIt);
    }

    public async Task Delete(int id)
    {
        PostIt? postIt = await _postItRepo.GetById(id);
        if (postIt == null)
            throw new KeyNotFoundException($"Post-It med ID {id} ikke fundet og kunne ikke slettes.");
        
        _postItRepo.DeleteById(postIt);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Post-It kunne ikke slettes. Prøv igen.");
    }

    // Helper method to map PostIt domain object to PostItDTO
    private PostItDTO MapToDTO(PostIt postIt)
    {
        return new PostItDTO
        {
            Id = postIt.Id,
            Date = postIt.Date,
            Payment = postIt.Payment,
            ShoppingDay = postIt.ShoppingDay,
            Mood = postIt.Mood,
            Status = postIt.Status,
            Events = postIt.Events,
            RelativesContact = postIt.RelativesContact,
            LastEditedByInitials = postIt.Staff?.Initials ?? string.Empty,

            // Include resident, staff and risk information

            ResidentId = postIt.ResidentId,
            ResidentInitials = postIt.Resident?.Initial ?? string.Empty,
            ResidentDepartmentId = postIt.Resident?.DepartmentId ?? 0,

            StaffName = postIt.Staff?.Initials ?? string.Empty,

            RiskId = postIt.Risk?.Id,
            RiskAssessment = postIt.Risk?.RiskAssessment ?? string.Empty,

            // If there are any PnTimes associated with the PostIt, map them to the DTO, otherwise return an empty list
            PnTimes = postIt.PnTimes.Select(p => new PnTimeDTO 
            { 
                Id = p.Id, 
                Time = p.Time, 
                Description = p.Description 
            }).ToList(),

            // If there are any Medicines associated with the PostIt, map them to the DTO, otherwise return an empty list
            Medicines = postIt.Medicines.Select(m => new MedicineDto 
            { 
                Id = m.Id, 
                Description = m.Description,
                TimeStamp = m.TimeStamp,
                CreatedAt = m.CreatedAt,
            }).ToList(),

        };
    }
}
