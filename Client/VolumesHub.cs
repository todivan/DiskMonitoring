using Common.Interfaces;
using Common.Model;
using Microsoft.AspNetCore.SignalR;

namespace Client;

public sealed class VolumesHub : Hub<IVolumes>
{
    public async Task SendMessage(IEnumerable<VolumeDisksReport> results)
    {
        await Clients.All.ShowResults(results);
    }
}

