using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Slottet.Application.Interfaces;


namespace Slottet.Application.BackgroundServices;

public class ResidentRetentionService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

// Needs scope factory to create a new scope, as background services are singleton and cannot directly inject scoped services like repositories or unit of work.
public ResidentRetentionService(IServiceScopeFactory scopeFactory)
{
    _scopeFactory = scopeFactory;
}

// Runs in background and checks for archived residents that are older than 6 months, and permanently deletes them from the database.
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        using var scope = _scopeFactory.CreateScope();

        var residentRepository = scope.ServiceProvider.GetRequiredService<IResidentRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var thresholdDate = DateTime.UtcNow.AddMonths(-6);
        var expiredResidents = await residentRepository.GetArchivedOlderThanAsync(thresholdDate, stoppingToken);

        if (expiredResidents.Any())
        {
            await residentRepository.RemoveRangeAsync(expiredResidents, stoppingToken);
            await unitOfWork.SaveChangesAsync();
        }

        await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
    }
}
}