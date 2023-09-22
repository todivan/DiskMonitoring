
using InfrastrucutreModul;
using Interfaces;
using Interfaces.Model;

namespace Client;

internal class Scanner : IScanner
{
    private readonly VolumesWinApiScanner _volumesScanner;
    private readonly VolumesWmiScanner _volumesWmiScanner;
    private readonly DiskPartitioWmiScanner _diskPartitioWmiScanner;

    public Scanner(DiskPartitioWmiScanner diskScanner, VolumesWinApiScanner volumesScanner, VolumesWmiScanner volumesWmiScanner, DiskPartitioWmiScanner diskPartitioWmiScanner)
    {
        _volumesScanner = volumesScanner;
        _volumesWmiScanner = volumesWmiScanner;
        _diskPartitioWmiScanner = diskPartitioWmiScanner;
    }

    public IEnumerable<VolumeDisksReport> Scan()
    {
        var results = new List<VolumeDisksReport>() { };

        var wmiVolumes = _volumesWmiScanner.GetVolumes();

        var diskPartitions = _diskPartitioWmiScanner.GetDiskPartitions();


        //foreach (var volume in volumes)
        //{
        //    results.Add(new VolumeDisksReport("MyScanType", volume, "MyDisk"));
        //    var rr = _diskScanner.GetDisks(volume);
        //}

        return results;
    }
}

