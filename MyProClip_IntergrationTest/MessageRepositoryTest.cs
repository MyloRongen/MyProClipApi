using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Exceptions.Message;
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
    public class MessageRepositoryTest
    {
        private ApplicationDbContext _context;
        private MessageRepository _messageRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            SeedData();

            _messageRepository = new MessageRepository(_context);
        }

        private void SeedData()
        {
            List<ApplicationUser> defaultUsers =
            [
                new ApplicationUser
                {
                    Id = "0206A018-5AC6-492D-AB99-10105193D384",
                    Email = "test1@gmail.com",
                    NormalizedEmail = "TEST1@GMAIL.COM",
                    UserName = "TestUser1",
                    NormalizedUserName = "TESTUSER1",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Password123!"),
                },
                new ApplicationUser
                {
                    Id = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                    Email = "test2@gmail.com",
                    NormalizedEmail = "TEST2@GMAIL.COM",
                    UserName = "TestUser2",
                    NormalizedUserName = "TESTUSER2",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Password123!"),
                },
                new ApplicationUser
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
        public async Task CreateMessageAsync_CreatesMessageSuccessfully()
        {
            // Arrange
            Message message = new()
            {
                SenderId = "0206A018-5AC6-492D-AB99-10105193D384",
                ReceiverId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738",
                Content = "Hello, how are you?",
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            Message? createdMessage = await _messageRepository.CreateMessageAsync(message);

            // Assert
            Assert.That(createdMessage, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(createdMessage.SenderId, Is.EqualTo(message.SenderId));
                Assert.That(createdMessage.ReceiverId, Is.EqualTo(message.ReceiverId));
                Assert.That(createdMessage.Content, Is.EqualTo(message.Content));
            });
        }

        [Test]
        public async Task GetMessagesAsync_ReturnsMessagesSuccessfully()
        {
            // Arrange
            string userId = "0206A018-5AC6-492D-AB99-10105193D384";
            string friendId = "0102B020-6BD7-4F9A-A632-45A1B7F0A738";

            List<Message> messagesToAdd =
            [
                new Message
                {
                    SenderId = userId,
                    ReceiverId = friendId,
                    Content = "Hello, how are you?",
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                },
                new Message
                {
                    SenderId = friendId,
                    ReceiverId = userId,
                    Content = "Hi, I'm doing well. How about you?",
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            ];

            await _context.Messages.AddRangeAsync(messagesToAdd);
            await _context.SaveChangesAsync();

            // Act
            List<Message> messages = await _messageRepository.GetMessagesAsync(userId, friendId);

            // Assert
            Assert.That(messages, Is.Not.Null);
            Assert.That(messages, Is.Not.Empty);
        }
    }
}
