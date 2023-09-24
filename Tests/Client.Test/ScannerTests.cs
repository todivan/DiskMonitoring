using Client;
using Common.Interfaces;
using Common.Model;
using InfrastrucutreModul;
using Microsoft.Extensions.Logging;
using Moq;

namespace Client.Test
{
    public class ScannerTests : IClassFixture<ScannerTestFixture>
    {
        private readonly ScannerTestFixture _fixture;

        public ScannerTests(ScannerTestFixture fixture) 
        {
            _fixture = new ScannerTestFixture();
        }

        [Fact]
        public void ScannerTests_Scan_success()
        {
            // Arrange
            var volumeId = "volumeId";
            IEnumerable<VolumesWinApiResults> volumesWinApiResults = new List<VolumesWinApiResults>()
            {
                new VolumesWinApiResults(){ VolumeId = "volumeId" }
            };
            _fixture.VolumesWinApiScanner.Setup(x => x.GetVolumes()).Returns(volumesWinApiResults);

            var driveLetter = "C:";
            IEnumerable<VolumesWmiResults> volumesWmiResults = new List<VolumesWmiResults>()
            {
                new VolumesWmiResults(){ DeviceID = volumeId, DriveLetter = driveLetter }
            };
            _fixture.VolumesWmiScanner.Setup(x => x.GetVolumes()).Returns(volumesWmiResults);

            var diskPartitionWmiResult = new DiskPartitionWmiResults()
            {
                StartingOffset = 1,
                DriveLetter = driveLetter,
                BlockSize = 2,
                DiskDescription = "description",
                DiskId = volumeId,
                DiskSize = 3,
                PartitionSize = 4
            };

            IEnumerable<DiskPartitionWmiResults> diskPartitionWmiResults = new List<DiskPartitionWmiResults>() { diskPartitionWmiResult };
            _fixture.DiskPartitionWmiScanner.Setup(x => x.GetDiskPartitions()).Returns(diskPartitionWmiResults);

            // Act
            var resutls = _fixture.Scanner.Scan();

            // Assert
            Assert.Single(resutls);
            var res0 = resutls.ElementAt(0);
            Assert.Equal(driveLetter, res0.DriveLetter);
            Assert.Equal(volumeId, res0.VolumeId);
            Assert.Equal(diskPartitionWmiResult.PartitionSize, res0.PartitionSize);
            Assert.Equal(diskPartitionWmiResult.DiskSize, res0.DiskSize);
            Assert.Equal(diskPartitionWmiResult.BlockSize, res0.BlockSize);
            Assert.Equal(diskPartitionWmiResult.StartingOffset, res0.StartingOffset);
            Assert.Equal(diskPartitionWmiResult.DiskId, res0.DiskId);
            Assert.Equal(diskPartitionWmiResult.DiskDescription, res0.DiskDescription);
        }

        [Fact]
        public void ScannerTests_Scan_VolumesWmiApiExc()
        {
            // Arrange
            _fixture.VolumesWmiScanner.Setup(x => x.GetVolumes()).Throws(new Exception());

            // Act
            var resutls = _fixture.Scanner.Scan();

            // Assert
            Assert.Empty(resutls);
            _fixture.Logger.Verify(logger => logger.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void ScannerTests_Scan_VolumesWinApiExc()
        {
            // Arrange
            _fixture.VolumesWinApiScanner.Setup(x => x.GetVolumes()).Throws(new Exception());

            // Act
            var resutls = _fixture.Scanner.Scan();

            // Assert
            Assert.Empty(resutls);
            _fixture.Logger.Verify(logger => logger.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void ScannerTests_Scan_DiskPartitionWmiExc()
        {
            // Arrange
            _fixture.DiskPartitionWmiScanner.Setup(x => x.GetDiskPartitions()).Throws(new Exception());

            // Act
            var resutls = _fixture.Scanner.Scan();

            // Assert
            Assert.Empty(resutls);
            _fixture.Logger.Verify(logger => logger.Log(LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}