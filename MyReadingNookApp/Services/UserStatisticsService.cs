using Microsoft.EntityFrameworkCore;
using MyReadingNookApp.Models;

namespace MyReadingNookApp.Services;

public class UserStatisticsService : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(30);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UserStatisticsService> _logger;

    public UserStatisticsService(IServiceScopeFactory scopeFactory, ILogger<UserStatisticsService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await UpdateUserStatsAsync(stoppingToken);

        using PeriodicTimer timer = new PeriodicTimer(_period);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await UpdateUserStatsAsync(stoppingToken);
        }
    }

    private async Task UpdateUserStatsAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var nookContext = scope.ServiceProvider.GetRequiredService<NookContext>();

            int affectedRows = await nookContext.Users
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.TotalBookCount, 
                        u => nookContext.UserBooks.Count(ub => ub.UserId == u.Id)) // Use UserId FK property directly
                    .SetProperty(u => u.PendingBookCount, 
                        u => nookContext.UserBooks.Count(ub => ub.UserId == u.Id && ub.BookStatus != "Completed"))
                    .SetProperty(u => u.CompletedBookCount, 
                        u => nookContext.UserBooks.Count(ub => ub.UserId == u.Id && ub.BookStatus == "Completed")),
                    stoppingToken);

            _logger.LogInformation("Successfully updated stats for {Count} users.", affectedRows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user statistics.");
        }
    }
}