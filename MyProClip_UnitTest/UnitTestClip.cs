using Moq;
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
    [TestFixture]
    public class UnitTestClip
    {
        [Test]
        public async Task GetClipsByUserId_ValidUserId_ReturnsClips()
        {
            // Arrange
            var userId = "9da57e2a-c9f2-4b69-9c88-68c2d19d89c5";
            var expectedClips = new List<Clip> { new() { Id = 1 }, new() { Id = 2 } };
            var mockRepository = new Mock<IClipRepository>();
            mockRepository.Setup(repo => repo.GetClipsByUserId(userId)).ReturnsAsync(expectedClips);
            var clipService = new ClipService(mockRepository.Object);

            // Act
            var result = await clipService.GetClipsByUserId(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedClips));
        }

        [Test]
        public void GetClipsByUserId_EmptyUserId_ThrowsException()
        {
            // Arrange
            var userId = "";
            var mockRepository = new Mock<IClipRepository>();
            var clipService = new ClipService(mockRepository.Object);

            // Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await clipService.GetClipsByUserId(userId));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("User ID does not exist"));
        }
    }
}
