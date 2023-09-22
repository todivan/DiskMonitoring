﻿
using System;

namespace InfrastrucutreModul.Models
{
    public class DiskPartitionWmiResults
    {
        public string DeviceID { get; set; }
        public UInt64 DiskSize { get; set; }
        public string DiskDescription { get; set; }
        public UInt64 PartitionSize { get; set; }
        public UInt64 BlockSize { get; set; }
        public UInt64 StartingOffset { get; set; }
    }
}