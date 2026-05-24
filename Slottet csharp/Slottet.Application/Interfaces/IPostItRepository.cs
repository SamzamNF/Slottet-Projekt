using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IPostItRepository
{
    Task Create(PostIt postIt);
    Task<PostIt?> GetById(int id);
    Task<List<PostIt>> GetByDate(DateTime date);
    Task<List<PostIt>> GetAll();
    Task<PostIt> Update(PostIt postIt);
    Task DeleteById(int id);
}