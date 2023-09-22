
using Interfaces.Model;

namespace Interfaces;

public interface IScanner
{
    IEnumerable<VolumeDisksReport> Scan();
}

