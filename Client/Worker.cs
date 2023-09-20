using Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<VolumesHub, IVolumes> _diskHub;

        public Worker(ILogger<Worker> logger, IHubContext<VolumesHub, IVolumes> diskHub)
        {
            _logger = logger;
            _diskHub = diskHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await _diskHub.Clients.All.ShowVolumes(new List<string>() { "Test Vol" });
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}