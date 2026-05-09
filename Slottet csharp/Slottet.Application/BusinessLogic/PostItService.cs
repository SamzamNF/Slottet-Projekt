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
            postItDTO.RelativesContact
        );

        await _postItRepo.Create(postIt);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Kunne ikke oprette PostIt.");

        return MapToDTO(postIt);
    }

    public async Task<PostItDTO> Update(PostItDTO postItDTO)
    {
        var postIt = PostIt.Create(
            postItDTO.Date,
            postItDTO.Payment,
            postItDTO.ShoppingDay,
            postItDTO.Mood,
            postItDTO.Status,
            postItDTO.Events,
            postItDTO.RelativesContact
        );

        postIt.Id = postItDTO.Id;

        var updatedPostIt = await _postItRepo.Update(postIt);

        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new InvalidOperationException("Post-It kunne ikke opdateres. Prøv igen.");

        return MapToDTO(updatedPostIt);
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
        // Call repository
        List<PostIt> postIts = await _postItRepo.GetAll();

        // Return list of DTOs based on list of domain objects
        return postIts.Select(postIt => MapToDTO(postIt)).ToList();
    }

    public async Task Delete(PostItDTO postItDTO)
    {
        await _postItRepo.DeleteById(postItDTO.Id);

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
            RelativesContact = postIt.RelativesContact
        };
    }
}
