using Common.Model;
using System.Collections.Generic;

namespace Common.Interfaces
{

    public interface IScanner
    {
        IEnumerable<VolumeDisksReport> Scan();
    }
}

