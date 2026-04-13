using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic
{
    public class PostItService
    {
        private readonly IPostItRepository _postItRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PostItService(IPostItRepository postItRepo, IUnitOfWork unitOfWork)
        {
            _postItRepo = postItRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<PostItDTO> UpdateInfo(PostItDTO postItDTO)
        {
            // Convert DTO to domain object
            var postIt = PostIt.Create(
                postItDTO.Date,
                postItDTO.Payment,
                postItDTO.ShoppingDay,
                postItDTO.Mood,
                postItDTO.Status,
                postItDTO.Events,
                postItDTO.RelativesContact
            );

            // Call repository
            var updatedPostIt = await _postItRepo.UpdatePostIt(postIt);

            int result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new InvalidOperationException("Kunne ikke gemme de indtastede oplysninger. Prøv igen.");

            // Return new DTO based on updated domain object
            return new PostItDTO
            {
                Date = updatedPostIt.Date,
                Payment = updatedPostIt.Payment,
                ShoppingDay = updatedPostIt.ShoppingDay,
                Mood = updatedPostIt.Mood,
                Status = updatedPostIt.Status,
                Events = updatedPostIt.Events,
                RelativesContact = updatedPostIt.RelativesContact
            };
        }
    }
}
