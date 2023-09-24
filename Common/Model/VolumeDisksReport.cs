
using System;

namespace Common.Model
{
    public sealed class VolumeDisksReport
    {
        public string VolumeId { get; set; }
        public string DiskId { get; set; }
        public string DriveLetter { get; set; }
        public string DiskDescription { get; set; }
        public UInt64 DiskSize { get; set; }
        public UInt64 PartitionSize { get; set; }
        public UInt64 BlockSize { get; set; }
        public UInt64 StartingOffset { get; set; }
    }
}


