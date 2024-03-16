using Microsoft.AspNetCore.Identity;
using Moq;
using MyProClip_BLL.Exceptions.User;
using MyProClip_BLL.Interfaces.Repositories;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_UnitTest
{
    [TestFixture]
    public class UnitTestUser
    {
        private IUserService _userService;
        private Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Test]
        public async Task FindUserByNameAsync_WithValidUsername_ReturnsUser()
        {
            // Arrange
            string username = "testUser";
            IdentityUser expectedUser = new() { UserName = username };
            _mockUserRepository.Setup(repo => repo.FindUserByNameAsync(username)).ReturnsAsync(expectedUser);

            // Act
            IdentityUser? result = await _userService.FindUserByNameAsync(username);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserName, Is.EqualTo(username));
        }

        [Test]
        public void FindUserByNameAsync_WithNullOrEmptyUsername_ThrowsInvalidUsernameException()
        {
            // Arrange
            string emptyUsername = "";
            string? nullUsername = null;

            // Assert & Act
            Assert.ThrowsAsync<InvalidUsernameException>(async () => await _userService.FindUserByNameAsync(emptyUsername));
            Assert.ThrowsAsync<InvalidUsernameException>(async () => await _userService.FindUserByNameAsync(nullUsername));
        }
    }
}
