using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SachkovTech.Issues.Infrastructure.Services;

namespace SachkovTech.Issues.Infrastructure.BackgroundServices;

public class DeleteExpiredIssuesBackgroundService : BackgroundService
{
    private readonly ILogger<DeleteExpiredIssuesBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public DeleteExpiredIssuesBackgroundService(
        ILogger<DeleteExpiredIssuesBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteExpiredIssuesBackgroundService is started");

        while (!cancellationToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            
            var deleteExpiredIssuesService = scope.ServiceProvider
                .GetRequiredService<DeleteExpiredIssuesService>();
            
            _logger.LogInformation("DeleteExpiredIssuesBackgroundService is working");
            
            await deleteExpiredIssuesService.Process(cancellationToken);
            
            await Task.Delay(
                TimeSpan.FromHours(Constants.DELETE_EXPIRED_ISSUES_SERVICE_REDUCTION_HOURS),
                cancellationToken);
        }
    }
}