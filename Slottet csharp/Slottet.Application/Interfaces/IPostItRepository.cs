using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IPostItRepository
{
    Task<PostIt> UpdatePostIt(PostIt postIt);
    Task<List<PostIt>> GetByDate(DateTime date);
    Task<List<PostIt>> GetAll();
}