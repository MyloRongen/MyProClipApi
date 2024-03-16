using Moq;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Exceptions.Friendship;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Interfaces.Services;
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
    public class UnitTestFriendship
    {
        private IFriendshipService _friendshipService;
        private Mock<IFriendshipRepository> _mockFriendshipRepository;

        [SetUp]
        public void Setup()
        {
            _mockFriendshipRepository = new Mock<IFriendshipRepository>();
            _friendshipService = new FriendshipService(_mockFriendshipRepository.Object);
        }

        [Test]
        public async Task CreateFriendship_ValidFriendship_CreatesFriendship()
        {
            // Arrange
            FriendShip friendship = new() { UserId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a", FriendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81" };

            // Act
            await _friendshipService.CreateFriendship(friendship);

            // Assert
            _mockFriendshipRepository.Verify(repo => repo.CreateFriendship(friendship), Times.Once);
        }

        [Test]
        public void CreateFriendship_NullOrEmptyArguments_ThrowsInvalidFriendshipDataException()
        {
            // Assert & Act
            Assert.ThrowsAsync<InvalidFriendshipDataException>(async () => await _friendshipService.CreateFriendship(new FriendShip()));
        }

        [Test]
        public async Task FriendshipExists_ValidIds_ReturnsTrue()
        {
            // Arrange
            string userId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a";
            string friendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81";
            _mockFriendshipRepository.Setup(repo => repo.FriendshipExists(userId, friendId)).ReturnsAsync(true);

            // Act
            bool result = await _friendshipService.FriendshipExists(userId, friendId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void FriendshipExists_NullUserId_ThrowsInvalidFriendIdException()
        {
            // Arrange
            string? userId = null;
            string friendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81";

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidUserIdException>(async () => await _friendshipService.FriendshipExists(userId, friendId));
            Assert.That(exception.Message, Is.EqualTo("Invalid user id."));
        }

        [Test]
        public void FriendshipExists_NullFriendId_ThrowsInvalidFriendIdException()
        {
            // Arrange
            string userId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a";
            string? friendId = null;

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidFriendIdException>(async () => await _friendshipService.FriendshipExists(userId, friendId));
            Assert.That(exception.Message, Is.EqualTo("Invalid friend id."));
        }

        [Test]
        public async Task GetPendingFriendRequests_ValidUserId_ReturnsRequests()
        {
            // Arrange
            string userId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a";
            List<FriendShip> expectedRequests = [new() { UserId = userId, FriendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81" }];
            _mockFriendshipRepository.Setup(repo => repo.GetPendingFriendRequests(userId)).ReturnsAsync(expectedRequests);

            // Act
            List<FriendShip> result = await _friendshipService.GetPendingFriendRequests(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedRequests));
        }

        [Test]
        public void GetPendingFriendRequests_NullOrEmptyUserId_ThrowsInvalidUserIdException()
        {
            // Assert & Act
            Assert.ThrowsAsync<InvalidUserIdException>(async () => await _friendshipService.GetPendingFriendRequests(""));
        }

        [Test]
        public async Task AcceptFriendRequest_ValidFriendship_AcceptsRequest()
        {
            // Arrange
            var friendship = new FriendShip { UserId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a", FriendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81" };

            // Act
            await _friendshipService.AcceptFriendRequest(friendship);

            // Assert
            Assert.That(friendship.Status, Is.EqualTo(FriendshipStatus.Accepted));
            _mockFriendshipRepository.Verify(repo => repo.AcceptFriendRequest(friendship), Times.Once);
        }

        [Test]
        public void AcceptFriendRequest_NullOrEmptyArguments_ThrowsInvalidFriendshipDataException()
        {
            // Assert & Act
            Assert.ThrowsAsync<InvalidFriendshipDataException>(async () => await _friendshipService.AcceptFriendRequest(new FriendShip()));
        }

        [Test]
        public async Task GetFriendshipByUserId_ValidId_ReturnsFriendship()
        {
            // Arrange
            int friendshipId = 123;
            FriendShip expectedFriendship = new() { UserId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a", FriendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81" };
            _mockFriendshipRepository.Setup(repo => repo.GetFriendshipByUserId(friendshipId)).ReturnsAsync(expectedFriendship);

            // Act
            FriendShip? result = await _friendshipService.GetFriendshipByUserId(friendshipId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedFriendship));
        }

        [Test]
        public void GetFriendshipByUserId_InvalidId_ThrowsFriendshipNotFoundException()
        {
            // Assert & Act
            Assert.ThrowsAsync<FriendshipNotFoundException>(async () => await _friendshipService.GetFriendshipByUserId(0));
        }

        [Test]
        public async Task DeleteFriendship_ValidFriendship_DeletesFriendship()
        {
            // Arrange
            FriendShip friendship = new() { UserId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a", FriendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81" };

            // Act
            await _friendshipService.DeleteFriendship(friendship);

            // Assert
            _mockFriendshipRepository.Verify(repo => repo.DeleteFriendship(friendship), Times.Once);
        }

        [Test]
        public void DeleteFriendship_NullOrEmptyArguments_ThrowsInvalidFriendshipDataException()
        {
            // Assert & Act
            Assert.ThrowsAsync<InvalidFriendshipDataException>(async () => await _friendshipService.DeleteFriendship(new FriendShip()));
        }

        [Test]
        public async Task GetFriendsById_ValidUserId_ReturnsFriends()
        {
            // Arrange
            string userId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a";
            List<FriendShip> expectedFriends =
            [
                new FriendShip { UserId = userId, FriendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81" },
                new FriendShip { UserId = userId, FriendId = "ede7ac2e-9b58-4b81-8c5d-6f470e496a82" },
                new FriendShip { UserId = userId, FriendId = "ad27ac2e-9b58-4b81-8c5d-6f470e496a84" }
            ];
            _mockFriendshipRepository.Setup(repo => repo.GetFriendsById(userId)).ReturnsAsync(expectedFriends);

            // Act
            List<FriendShip> result = await _friendshipService.GetFriendsById(userId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedFriends));
        }

        [Test]
        public void GetFriendsById_NullOrEmptyUserId_ThrowsInvalidUserIdException()
        {
            // Assert & Act
            Assert.ThrowsAsync<InvalidUserIdException>(async () => await _friendshipService.GetFriendsById(""));
        }
    }
}
