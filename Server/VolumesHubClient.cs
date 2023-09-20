using Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR.Client;

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

            _connection.On<IEnumerable<string>>(Config.Events.VolumesSent, ShowVolumes);
        }

        public Task ShowVolumes(IEnumerable<string> volumes)
        {
            _logger.LogInformation("{Volumes}", volumes.First());

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
