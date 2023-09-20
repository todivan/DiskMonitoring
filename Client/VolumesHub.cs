using Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Client;

public sealed class VolumesHub : Hub<IVolumes>
{
    public async Task SendMessage(IEnumerable<string> volumes)
    {
        await Clients.All.ShowVolumes(volumes);
    }
}

