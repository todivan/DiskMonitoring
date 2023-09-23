
using InfrastrucutreModul;
using InfrastrucutreModul.Models;
using Interfaces;
using Interfaces.Model;
using System.Runtime.InteropServices;

namespace Client;

internal class Scanner : IScanner
{
    private readonly VolumesWinApiScanner _volumesWinApiScanner;
    private readonly VolumesWmiScanner _volumesWmiScanner;
    private readonly DiskPartitioWmiScanner _diskPartitioWmiScanner;

    public Scanner(VolumesWinApiScanner volumesWinApiScanner, VolumesWmiScanner volumesWmiScanner, DiskPartitioWmiScanner diskPartitioWmiScanner)
    {
        _volumesWinApiScanner = volumesWinApiScanner;
        _volumesWmiScanner = volumesWmiScanner;
        _diskPartitioWmiScanner = diskPartitioWmiScanner;
    }

    public IEnumerable<VolumeDisksReport> Scan()
    {
        var winApiVolumes = _volumesWinApiScanner.GetVolumes();

        var wmiVolumes = _volumesWmiScanner.GetVolumes();

        var diskPartitions = _diskPartitioWmiScanner.GetDiskPartitions();

        IEnumerable<VolumeDisksReport> results = CrateResults(winApiVolumes, wmiVolumes, diskPartitions);

        return results;
    }

    private IEnumerable<VolumeDisksReport> CrateResults(List<VolumesWinApiResults> winApiVolumes, 
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

