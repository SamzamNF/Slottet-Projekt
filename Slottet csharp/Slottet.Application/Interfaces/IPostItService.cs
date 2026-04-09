using Slottet.Domain.Entities;

namespace Slottet.Application.Interfaces
{
    public interface IPostItService
    {
        Task UpdateInfo(PostIt postIt);
    }
}
