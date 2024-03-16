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
    [TestFixture]
    public class UnitTestClip
    {
        [Test]
        public async Task GetClipsByUserId_ValidUserId_ReturnsClips()
        {
            // Arrange
            string userId = "9da57e2a-c9f2-4b69-9c88-68c2d19d89c5";
            List<Clip> expectedClips = [new() { Id = 1 }, new() { Id = 2 }];
            Mock<IClipRepository> mockRepository = new();
            mockRepository.Setup(repo => repo.GetClipsByUserId(userId)).ReturnsAsync(expectedClips);
            ClipService clipService = new(mockRepository.Object);

            // Act
            List<Clip> result = await clipService.GetClipsByUserId(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedClips));
        }

        [Test]
        public void GetClipsByUserId_EmptyUserId_ThrowsArgumentException()
        {
            // Arrange
            string userId = "";
            Mock<IClipRepository> mockRepository = new();
            ClipService clipService = new(mockRepository.Object);

            // Act
            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await clipService.GetClipsByUserId(userId));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Does.Contain("User ID is required."));
                Assert.That(exception.ParamName, Is.EqualTo(nameof(userId)));
            });
        }

        [Test]
        public async Task GetPublicClips_ReturnsPublicClips()
        {
            // Arrange
            List<Clip> expectedClips = [new() { Id = 1 }, new() { Id = 2 }];
            Mock<IClipRepository> mockRepository = new();
            mockRepository.Setup(repo => repo.GetPublicClips()).ReturnsAsync(expectedClips);
            ClipService clipService = new(mockRepository.Object);

            // Act
            List<Clip> result = await clipService.GetPublicClips();

            // Assert
            Assert.That(result, Is.EqualTo(expectedClips));
        }

        [Test]
        public void AddClip_InvalidData_ThrowsInvalidClipDataException()
        {
            // Arrange
            Clip clip = new(); // Clip instance with no data
            Mock<IClipRepository> mockRepository = new();
            ClipService clipService = new(mockRepository.Object);

            // Act & Assert
            InvalidClipDataException exception = Assert.Throws<InvalidClipDataException>(() => clipService.AddClip(clip));
            Assert.That(exception.Message, Is.EqualTo("Invalid clip data. UserId, Title, video URL, and thumbnail URL are required."));
        }

        [Test]
        public void GetClipById_InvalidClipId_ThrowsArgumentException()
        {
            // Arrange
            int clipId = 0;
            Mock<IClipRepository> mockRepository = new();
            ClipService clipService = new(mockRepository.Object);

            // Act & Assert
            ArgumentException exception = Assert.ThrowsAsync<ArgumentException>(async () => await clipService.GetClipById(clipId));
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Does.Contain("Invalid clip ID."));
                Assert.That(exception.ParamName, Is.EqualTo(nameof(clipId)));
            });
        }

        [Test]
        public void DeleteClipAsync_NullClip_ThrowsArgumentNullException()
        {
            // Arrange
            Clip? clip = null;
            Mock<IClipRepository> mockRepository = new();
            ClipService clipService = new(mockRepository.Object);

            // Act & Assert
            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await clipService.DeleteClipAsync(clip));
            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Does.Contain("Clip instance is null."));
                Assert.That(exception.ParamName, Is.EqualTo(nameof(clip)));
            });
        }

        [Test]
        public void AddClip_ValidClip_AddsClip()
        {
            // Arrange
            Clip validClip = new()
            {
                UserId = "user_id",
                Title = "Title",
                VideoUrl = "video_url",
                ThumbnailUrl = "thumbnail_url"
            };
            Mock<IClipRepository> mockRepository = new();
            mockRepository.Setup(repo => repo.AddClip(validClip));
            ClipService clipService = new(mockRepository.Object);

            // Act & Assert
            Assert.DoesNotThrow(() => clipService.AddClip(validClip));
        }

        [Test]
        public async Task GetClipById_ValidClipId_ReturnsClip()
        {
            // Arrange
            int validClipId = 1;
            Clip expectedClip = new()
            {
                Id = validClipId,
                UserId = "user_id",
                Title = "Title",
                VideoUrl = "video_url",
                ThumbnailUrl = "thumbnail_url"
            };
            Mock<IClipRepository> mockRepository = new();
            mockRepository.Setup(repo => repo.GetClipById(validClipId)).ReturnsAsync(expectedClip);
            ClipService clipService = new(mockRepository.Object);

            // Act
            Clip? actualClip = await clipService.GetClipById(validClipId);

            // Assert
            Assert.That(actualClip, Is.EqualTo(expectedClip));
        }

        [Test]
        public void DeleteClipAsync_ValidClip_DeletesClip()
        {
            // Arrange
            Clip validClip = new()
            {
                Id = 1,
                UserId = "user_id",
                Title = "Title",
                VideoUrl = "video_url",
                ThumbnailUrl = "thumbnail_url"
            };
            Mock<IClipRepository> mockRepository = new();
            mockRepository.Setup(repo => repo.DeleteClipAsync(validClip)).Returns(Task.CompletedTask);
            ClipService clipService = new(mockRepository.Object);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await clipService.DeleteClipAsync(validClip));
        }
    }
}
