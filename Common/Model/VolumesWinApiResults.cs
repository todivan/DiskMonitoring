
namespace Common.Model
{
    public class VolumesWinApiResults
    {
        public string VolumeId { get; set; }
        public long ExtentLength { get; set; }
        public long StartingOffset { get; set; }
        public uint DiskNumber { get; set; }
    }
}
