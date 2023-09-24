
using Common.Interfaces;
using Common.Model;
using System.Collections.Generic;
using System.Management;

namespace InfrastrucutreModul
{

    public class VolumesWmiScanner : WmiScannerBase, IVolumesWmiScanner
    {
        public IEnumerable<VolumesWmiResults> GetVolumes()
        {
            var result = new List<VolumesWmiResults>();

            var moCollection = ExecuteCommand("Select DeviceID, DriveLetter from Win32_Volume");
            foreach (ManagementObject mo in moCollection)
            {
                var id = mo["DeviceID"]?.ToString();
                var letter = mo["DriveLetter"]?.ToString();

                result.Add(new VolumesWmiResults()
                {
                    DeviceID = id,
                    DriveLetter = letter,
                });
            }

            return result;
        }
    }
}

