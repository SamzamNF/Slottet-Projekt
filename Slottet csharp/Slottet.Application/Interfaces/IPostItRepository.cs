using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces
{
    public interface IPostItRepository
    {
        Task UpdatePostIt(PostIt postIt);
    }  
}
