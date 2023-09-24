using Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Client
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<VolumesHub, IVolumes> _diskHub;
        private readonly IScanner _scanner;

        public Worker(ILogger<Worker> logger, IHubContext<VolumesHub, IVolumes> diskHub, IScanner scanner)
        {
            _logger = logger;
            _diskHub = diskHub;
            _scanner = scanner;   
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var results = _scanner.Scan();
                await _diskHub.Clients.All.ShowResults(results);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}