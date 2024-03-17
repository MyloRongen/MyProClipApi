using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Models;
using MyProClip_DAL.Data;
using MyProClip_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProClip_IntergrationTest
{
    public class FriendshipRepositoryTest
    {
        private ApplicationDbContext _context;
        private FriendshipRepository _friendshipRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            SeedData();

            _friendshipRepository = new FriendshipRepository(_context);
        }

        private void SeedData()
        {
            List<IdentityUser> defaultUsers =
            [
                new IdentityUser
                {
                    Id = "0206A018-5AC6-492D-AB99-10105193D384",
                    Email = "test1@gmail.com",
                    NormalizedEmail = "TEST1@GMAIL.COM",
                    UserName = "TestUser1",
                    NormalizedUserName = "TESTUSER1",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Password123!"),
                },
                new IdentityUser
                {
                    Id = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                    Email = "test2@gmail.com",
                    NormalizedEmail = "TEST2@GMAIL.COM",
                    UserName = "TestUser2",
                    NormalizedUserName = "TESTUSER2",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Password123!"),
                },
                new IdentityUser
                {
                    Id = "0303C030-8EE8-4D4F-BE7C-6A7E8E1E1E23",
                    Email = "test3@gmail.com",
                    NormalizedEmail = "TEST3@GMAIL.COM",
                    UserName = "TestUser3",
                    NormalizedUserName = "TESTUSER3",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Password123!"),
                }
            ];

            _context.Users.AddRange(defaultUsers);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task CreateFriendship_AddsFriendshipToDatabase()
        {
            // Arrange
            FriendShip newFriendship = new FriendShip
            {
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                FriendId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                Status = FriendshipStatus.Pending
            };

            // Act
            await _friendshipRepository.CreateFriendship(newFriendship);

            // Assert
            FriendShip? savedFriendship = await _context.Friendships.FirstOrDefaultAsync(f => f.UserId == newFriendship.UserId && f.FriendId == newFriendship.FriendId);
            Assert.That(savedFriendship, Is.Not.Null);
        }

        [Test]
        public async Task FriendshipExists_Returns_True_WhenFriendshipExists()
        {
            // Arrange
            string userId = "0206A018-5AC6-492D-AB99-10105193D384";
            string friendId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738";
            FriendShip existingFriendship = new()
            {
                UserId = userId,
                FriendId = friendId,
                Status = FriendshipStatus.Accepted
            };

            await _context.Friendships.AddAsync(existingFriendship);
            await _context.SaveChangesAsync();

            // Act
            bool exists = await _friendshipRepository.FriendshipExists(userId, friendId);

            // Assert
            Assert.That(exists, Is.True);
        }

        [Test]
        public async Task GetPendingFriendRequests_Returns_CorrectRequests()
        {
            // Arrange
            string userId = "0206A018-5AC6-492D-AB99-10105193D384";
            FriendShip pendingRequest = new()
            {
                UserId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                FriendId = userId,
                Status = FriendshipStatus.Pending
            };

            await _context.Friendships.AddAsync(pendingRequest);
            await _context.SaveChangesAsync();

            // Act
            List<FriendShip> pendingRequests = await _friendshipRepository.GetPendingFriendRequests(userId);

            // Assert
            Assert.That(pendingRequests, Is.Not.Null);
            Assert.That(pendingRequests, Has.Count.EqualTo(1));
            Assert.That(pendingRequests[0].FriendId, Is.EqualTo(userId));
        }

        [Test]
        public async Task AcceptFriendRequest_UpdatesFriendshipStatus()
        {
            // Arrange
            FriendShip friendshipToAccept = new()
            {
                UserId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                FriendId = "0206A018-5AC6-492D-AB99-10105193D384",
                Status = FriendshipStatus.Pending
            };

            await _context.Friendships.AddAsync(friendshipToAccept);
            await _context.SaveChangesAsync();

            // Act
            friendshipToAccept.Status = FriendshipStatus.Accepted;
            await _friendshipRepository.AcceptFriendRequest(friendshipToAccept);

            // Assert
            FriendShip? updatedFriendship = await _context.Friendships.FirstOrDefaultAsync(f => f.UserId == friendshipToAccept.UserId && f.FriendId == friendshipToAccept.FriendId);
            Assert.That(updatedFriendship, Is.Not.Null);
            Assert.That(updatedFriendship.Status, Is.EqualTo(FriendshipStatus.Accepted));
        }

        [Test]
        public async Task DeleteFriendship_RemovesFriendshipFromDatabase()
        {
            // Arrange
            FriendShip friendshipToDelete = new()
            {
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                FriendId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                Status = FriendshipStatus.Accepted
            };

            await _context.Friendships.AddAsync(friendshipToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _friendshipRepository.DeleteFriendship(friendshipToDelete);

            // Assert
            FriendShip? deletedFriendship = await _context.Friendships.FindAsync(friendshipToDelete.Id);
            Assert.That(deletedFriendship, Is.Null);
        }

        [Test]
        public async Task GetFriendshipByUserId_Returns_Friendship_WhenExists()
        {
            // Arrange
            FriendShip newFriendship = new()
            {
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                FriendId = "FriendId",
                Status = FriendshipStatus.Accepted
            };

            await _context.Friendships.AddAsync(newFriendship);
            await _context.SaveChangesAsync();

            // Act
            FriendShip? retrievedFriendship = await _friendshipRepository.GetFriendshipByUserId(newFriendship.Id);

            // Assert
            Assert.That(retrievedFriendship, Is.Not.Null);
            Assert.That(retrievedFriendship.Id, Is.EqualTo(newFriendship.Id));
        }

        [Test]
        public async Task GetFriendshipByUserId_Returns_Null_WhenNotExists()
        {
            // Act
            FriendShip? retrievedFriendship = await _friendshipRepository.GetFriendshipByUserId(-1);

            // Assert
            Assert.That(retrievedFriendship, Is.Null);
        }

        [Test]
        public async Task GetFriendsById_Returns_CorrectFriends()
        {
            // Arrange
            string userId = "0206A018-5AC6-492D-AB99-10105193D384";
            FriendShip friendship1 = new()
            {
                UserId = userId,
                FriendId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                Status = FriendshipStatus.Accepted
            };
            FriendShip friendship2 = new()
            {
                UserId = "0303C030-8EE8-4D4F-BE7C-6A7E8E1E1E23",
                FriendId = userId,
                Status = FriendshipStatus.Accepted
            };

            await _context.Friendships.AddRangeAsync(new List<FriendShip> { friendship1, friendship2 });
            await _context.SaveChangesAsync();

            // Act
            List<FriendShip> friends = await _friendshipRepository.GetFriendsById(userId);

            // Assert
            Assert.That(friends, Is.Not.Null);
            Assert.That(friends, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(friends.Exists(f => f.FriendId == "0102B020-6BD7-4F9A-A632-45A1B7F0A738"), Is.True);
                Assert.That(friends.Exists(f => f.UserId == "0303C030-8EE8-4D4F-BE7C-6A7E8E1E1E23"), Is.True);
            });
        }
    }
}
