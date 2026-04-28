using Microsoft.Extensions.Hosting;
using Slottet.Application.Interfaces;

namespace Slottet.Application.BackgroundServices;

public class ResidentRetentionService : BackgroundService
{
    private readonly IResidentRepository _residentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResidentRetentionService(IResidentRepository residentRepository, IUnitOfWork unitOfWork)
    {
        _residentRepository = residentRepository;
        _unitOfWork = unitOfWork;
    }

    // Runs in background and checks for archived residents that are older than 6 months, and permanently deletes them from the database.
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var thresholdDate = DateTime.UtcNow.AddMonths(-6);

            var expiredResidents = await _residentRepository.GetArchivedOlderThanAsync(thresholdDate, stoppingToken);

            if (expiredResidents.Any())
            {
                await _residentRepository.RemoveRangeAsync(expiredResidents, stoppingToken);
                await _unitOfWork.SaveChangesAsync();
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}