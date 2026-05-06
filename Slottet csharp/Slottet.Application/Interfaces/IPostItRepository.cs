using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IPostItRepository
{
    Task CreatePostIt(PostIt postIt);
    Task<List<PostIt>> GetPostItByDate(DateTime date);
    Task<List<PostIt>> GetAllPostIts();
    Task<PostIt> UpdatePostIt(PostIt postIt);
    Task DeletePostItById(int id);
}