using Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Test
{
    public class ScannerTestFixture
    {
        public Mock<ILogger<Scanner>> Logger { get; }
        public Mock<IVolumesWinApiScanner> VolumesWinApiScanner { get; }
        public Mock<IVolumesWmiScanner> VolumesWmiScanner { get; }
        public Mock<IDiskPartitionWmiScanner> DiskPartitionWmiScanner { get; }
        public Scanner Scanner;

        public ScannerTestFixture()
        {
            Logger = new Mock<ILogger<Scanner>>();
            VolumesWinApiScanner = new Mock<IVolumesWinApiScanner>();
            VolumesWmiScanner = new Mock<IVolumesWmiScanner>();
            DiskPartitionWmiScanner = new Mock<IDiskPartitionWmiScanner>();

            Scanner = new Scanner(Logger.Object, VolumesWinApiScanner.Object, VolumesWmiScanner.Object, DiskPartitionWmiScanner.Object);
        }
    }
}
