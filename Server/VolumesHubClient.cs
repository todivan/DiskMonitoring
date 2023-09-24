using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Common.Interfaces;
using Common.Model;
using Common;

namespace Server
{
    internal class VolumesHubClient : IVolumes, IHostedService
    {
        private readonly ILogger<VolumesHubClient> _logger;
        private HubConnection _connection;

        public VolumesHubClient(ILogger<VolumesHubClient> logger)
        {
            _logger = logger;

            _connection = new HubConnectionBuilder()
            .WithUrl(Config.HubUrl)
                .Build();

            _connection.On<IEnumerable<VolumeDisksReport>>(Config.Events.VolumesSent, ShowResults);
        }

        public Task ShowResults(IEnumerable<VolumeDisksReport> reports)
        {
            foreach (var report in reports)
            {
                _logger.LogInformation($"VolumeId:{report.VolumeId}, DriveLetter:{report.DriveLetter}, DiskId:{report.DiskId}, " +
                    $"DiskDescription:{report.DiskDescription}, DiskSize:{report.DiskSize}, PartitionSize:{report.PartitionSize}, " +
                    $"StartingOffset:{report.StartingOffset}, BlockSize:{report.BlockSize}");
            }

            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    await _connection.StartAsync(cancellationToken);

                    break;
                }
                catch
                {
                    await Task.Delay(5000, cancellationToken);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.DisposeAsync();
        }
    }
}
