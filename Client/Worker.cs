using Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<VolumesHub, IVolumes> _diskHub;
        private readonly IVolumesScanner _volumesScanner;

        public Worker(ILogger<Worker> logger, IHubContext<VolumesHub, IVolumes> diskHub, IVolumesScanner volumesScanner)
        {
            _logger = logger;
            _diskHub = diskHub;
            _volumesScanner = volumesScanner;   
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var volumes = _volumesScanner.GetVolumes();
                await _diskHub.Clients.All.ShowVolumes(new List<string>() { "Test Vol" });
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}