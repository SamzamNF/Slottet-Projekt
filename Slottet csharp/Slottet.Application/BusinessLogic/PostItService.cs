using System;
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

        public PostItService(IPostItRepository postItRepo)
        {
            _postItRepo = postItRepo;
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
