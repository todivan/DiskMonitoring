using Interfaces.Model;

namespace Interfaces;

public interface IVolumes
{
    Task ShowResults(IEnumerable<VolumeDisksReport> results);
}
