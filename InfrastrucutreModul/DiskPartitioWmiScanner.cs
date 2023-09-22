
using InfrastrucutreModul.Models;
using System;
using System.Collections.Generic;
using System.Management;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;

namespace InfrastrucutreModul
{

    public class DiskPartitioWmiScanner
    {
        public IEnumerable<DiskPartitionWmiResults> GetDiskPartitions()
        {
            var result = new List<DiskPartitionWmiResults>();

            ManagementObjectSearcher ms = new ManagementObjectSearcher("Select * from Win32_DiskPartition");
            foreach (ManagementObject mo in ms.Get())
            {
                UInt64 blockSize = (UInt64)(mo["BlockSize"] ?? 0);
                UInt64 startingOffset = (UInt64)(mo["StartingOffset"] ?? 0);
                UInt64 partitionSize = (UInt64)(mo["Size"] ?? 0);

                var moCollection = mo.GetRelated("Win32_DiskDrive");

                foreach (var moItem in moCollection)
                {
                    var deviceID = moItem["DeviceID"]?.ToString();
                    var description = moItem["Description"]?.ToString();
                    UInt64 diskSize = (UInt64)(moItem["Size"] ?? 0);

                    result.Add(new DiskPartitionWmiResults()
                    {
                        BlockSize = blockSize,
                        DeviceID = deviceID,
                        DiskDescription = description,
                        DiskSize = diskSize,
                        PartitionSize = partitionSize,
                        StartingOffset = startingOffset
                    });
                }
            }

            return result;
        }
    }
}

