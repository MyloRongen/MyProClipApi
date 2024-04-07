using Moq;
using MyProClip_BLL.Exceptions.Clip;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Models;
using MyProClip_BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_UnitTest
{
    public class UnitTestReportedClip
    {
        private ReportedClipService _reportedClipService;
        private Mock<IReportedClipRepository> _mockReportedClipRepository;

        [SetUp]
        public void Setup()
        {
            _mockReportedClipRepository = new Mock<IReportedClipRepository>();
            _reportedClipService = new ReportedClipService(_mockReportedClipRepository.Object);
        }

        [Test]
        public async Task GetReportedClips_Success()
        {
            // Arrange
            var reportedClips = new List<ReportUserClip>();
            _mockReportedClipRepository.Setup(repo => repo.GetReportedClips()).ReturnsAsync(reportedClips);

            // Act
            var result = await _reportedClipService.GetReportedClips();

            // Assert
            Assert.AreEqual(reportedClips, result);
        }

        [Test]
        public void GetReportUserClipById_ReportedClipIdZero_ThrowsException()
        {
            // Arrange
            int reportedClipId = 0;

            // Act & Assert
            Assert.ThrowsAsync<UserReportClipException>(async () =>
            {
                await _reportedClipService.GetReportUserClipById(reportedClipId);
            });
        }

        [Test]
        public async Task GetReportUserClipById_ValidReportedClipId_ReturnsReportUserClip()
        {
            // Arrange
            int reportedClipId = 1;
            var reportUserClip = new ReportUserClip { Id = reportedClipId };
            _mockReportedClipRepository.Setup(repo => repo.GetReportUserClipById(reportedClipId)).ReturnsAsync(reportUserClip);

            // Act
            var result = await _reportedClipService.GetReportUserClipById(reportedClipId);

            // Assert
            Assert.AreEqual(reportUserClip, result);
        }

        [Test]
        public async Task DeleteReportedClip_ValidReportUserClip_DeletesClip()
        {
            // Arrange
            var reportUserClip = new ReportUserClip { Id = 1 };

            // Act
            await _reportedClipService.DeleteReportedClip(reportUserClip);

            // Assert
            _mockReportedClipRepository.Verify(repo => repo.DeleteReportedClip(reportUserClip), Times.Once());
        }

        [Test]
        public void DeleteReportedClip_NullReportUserClip_ThrowsException()
        {
            // Arrange
            ReportUserClip reportUserClip = null;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _reportedClipService.DeleteReportedClip(reportUserClip);
            });
        }
    }
}
