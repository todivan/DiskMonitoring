
using Common.Interfaces;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace InfrastrucutreModul
{

    public class DiskPartitionWmiScanner : IDiskPartitionWmiScanner
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

                var moDiskDrive = mo.GetRelated("Win32_DiskDrive");
                var moLogicalDisk = mo.GetRelated("Win32_LogicalDisk");

                foreach (var moItem in moDiskDrive)
                {
                    var deviceID = moItem["DeviceID"]?.ToString();
                    var description = moItem["Description"]?.ToString();
                    UInt64 diskSize = (UInt64)(moItem["Size"] ?? 0);

                    string driveLetter = string.Empty;
                    var logicalDIsk = moLogicalDisk.OfType<ManagementObject>().FirstOrDefault();
                    if (logicalDIsk != null)
                    {
                        driveLetter = logicalDIsk["DeviceId"]?.ToString();
                    }

                    result.Add(new DiskPartitionWmiResults()
                    {
                        BlockSize = blockSize,
                        DriveLetter = driveLetter,
                        DiskId = deviceID,
                        DiskDescription = description,
                        DiskSize = diskSize,
                        PartitionSize = partitionSize,
                        StartingOffset = startingOffset,
                    });
                }
            }

            return result;
        }
    }
}

