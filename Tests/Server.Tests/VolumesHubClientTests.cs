using Microsoft.Extensions.Logging;
using Server;
using Moq;
using Common.Model;

namespace Server.Tests
{
    public class VolumesHubClientTests
    {
        [Fact]
        public void VolumesHubClient_Success()
        {
            // Arrange
            Mock<ILogger<VolumesHubClient>> logger = new Mock<ILogger<VolumesHubClient>>();

            VolumesHubClient volumesHubClient = new VolumesHubClient(logger.Object);

            VolumeDisksReport volumeDisksReport1 = new VolumeDisksReport()
            {
                BlockSize= 1024,
                DiskDescription = "desc1",
                DiskId = "Id1",
                DiskSize= 1,
                DriveLetter = "C:",
                PartitionSize= 22,
                StartingOffset= 333,
                VolumeId = "VolId1"
            };

            VolumeDisksReport volumeDisksReport2 = new VolumeDisksReport()
            {
                BlockSize = 2024,
                DiskDescription = "desc2",
                DiskId = "Id2",
                DiskSize = 21,
                DriveLetter = "D:",
                PartitionSize = 222,
                StartingOffset = 2333,
                VolumeId = "VolId2"
            };

            IEnumerable<VolumeDisksReport> volumeDisksReports  = new List<VolumeDisksReport>() { volumeDisksReport1, volumeDisksReport2 };

            // Act
            var result =  volumesHubClient.ShowResults(volumeDisksReports);

            // Assert
            logger.Verify(logger => logger.Log(LogLevel.Information, 
                    It.IsAny<EventId>(), 
                    It.IsAny<It.IsAnyType>(), 
                    null, (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Exactly(2));
            Assert.Equal(Task.CompletedTask, result);
        }
    }
}