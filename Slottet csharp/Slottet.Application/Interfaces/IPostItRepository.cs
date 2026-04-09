using Slottet.Domain.Entities;

public interface IPostItRepository
{
    Task UpdatePostIt(PostIt postIt);
}