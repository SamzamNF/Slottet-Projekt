using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces;

public interface IPostItRepository
{
    Task Create(PostIt postIt);
    Task<List<PostIt>> GetByDate(DateTime date);
    Task<List<PostIt>> GetAll();
    Task<PostIt> GetById(int id);
    void Update(PostIt postIt);
    void DeleteById(int id);
}