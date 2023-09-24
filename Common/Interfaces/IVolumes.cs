using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IVolumes
    {
        Task ShowResults(IEnumerable<VolumeDisksReport> results);
    }
}
