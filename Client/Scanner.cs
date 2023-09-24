
using Common.Interfaces;
using Common.Model;

namespace Client;

public class Scanner : IScanner
{
    private readonly ILogger<Scanner> _logger;
    private readonly IVolumesWinApiScanner _volumesWinApiScanner;
    private readonly IVolumesWmiScanner _volumesWmiScanner;
    private readonly IDiskPartitionWmiScanner _diskPartitioWmiScanner;

    public Scanner(ILogger<Scanner> logger, IVolumesWinApiScanner volumesWinApiScanner, IVolumesWmiScanner volumesWmiScanner, IDiskPartitionWmiScanner diskPartitioWmiScanner)
    {
        _logger = logger;
        _volumesWinApiScanner = volumesWinApiScanner;
        _volumesWmiScanner = volumesWmiScanner;
        _diskPartitioWmiScanner = diskPartitioWmiScanner;
    }

    public IEnumerable<VolumeDisksReport> Scan()
    {
        var winApiVolumes = ExecuteScanner<VolumesWinApiResults>(_volumesWinApiScanner.GetVolumes, "Failed to get volumes over WinApi");

        var wmiVolumes = ExecuteScanner<VolumesWmiResults>(_volumesWmiScanner.GetVolumes, "Failed to get volumes over Wmi");

        var diskPartitions = ExecuteScanner<DiskPartitionWmiResults>(_diskPartitioWmiScanner.GetDiskPartitions, "Failed to get disk partitions over Wmi");

        IEnumerable<VolumeDisksReport> results = CrateResults(winApiVolumes, wmiVolumes, diskPartitions);

        return results;
    }

    private IEnumerable<T> ExecuteScanner<T>(Func<IEnumerable<T>> action, string exceptionMethod)
    {
        IEnumerable<T> results = new List<T>();

        try
        {
            results = action();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, exceptionMethod);
        }

        return results;
    }

    private IEnumerable<VolumeDisksReport> CrateResults(IEnumerable<VolumesWinApiResults> winApiVolumes, 
        IEnumerable<VolumesWmiResults> wmiVolumes, 
        IEnumerable<DiskPartitionWmiResults> diskPartitions)
    {
        List<VolumeDisksReport> results = new List<VolumeDisksReport>();

        foreach (var winApiVolume in winApiVolumes)
        {
            VolumeDisksReport volumeDisksReport = new VolumeDisksReport();
            volumeDisksReport.VolumeId = winApiVolume.VolumeId;

            var wmiVolume = wmiVolumes.FirstOrDefault(x => x.DeviceID == winApiVolume.VolumeId);
            if (wmiVolume != null)
            {
                volumeDisksReport.DriveLetter = wmiVolume.DriveLetter;
                
                var diskPartition = diskPartitions.FirstOrDefault(x => !string.IsNullOrEmpty(x.DriveLetter) && x.DriveLetter == wmiVolume.DriveLetter);
                if (diskPartition != null)
                {
                    volumeDisksReport.DiskId = diskPartition.DiskId;
                    volumeDisksReport.DiskDescription = diskPartition.DiskDescription;
                    volumeDisksReport.DiskSize = diskPartition.DiskSize;
                    volumeDisksReport.PartitionSize = diskPartition.PartitionSize;
                    volumeDisksReport.BlockSize = diskPartition.BlockSize;
                    volumeDisksReport.StartingOffset = diskPartition.StartingOffset;
                }
            }

            results.Add(volumeDisksReport);
        }

        return results;
    }
}

