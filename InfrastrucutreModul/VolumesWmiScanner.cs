
using InfrastrucutreModul.Models;
using System.Collections.Generic;
using System.Management;

namespace InfrastrucutreModul
{

    public class VolumesWmiScanner
    {
        public IEnumerable<VolumesWmiResults> GetVolumes()
        {
            var result = new List<VolumesWmiResults>();

            ManagementObjectSearcher ms = new ManagementObjectSearcher("Select DeviceID, DriveLetter from Win32_Volume");
            var moCollection = ms.Get();
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

