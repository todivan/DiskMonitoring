using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;
using Interfaces.Model;

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
                _logger.LogInformation("{Volumes}", report.volume);
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
                    await Task.Delay(1000, cancellationToken);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.DisposeAsync();
        }
    }
}
