
namespace Interfaces;

public static class Config
{
    public static string action => "/hubs/volumes";

    public static string HubUrl => $"http://localhost:5000{action}";

    public static class Events
    {
        public static string VolumesSent => nameof(IVolumes.ShowResults);
    }
}

