using Moq;
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
    public class UnitTestMessage
    {
        private IMessageService _messageService;
        private Mock<IMessageRepository> _mockMessageRepository;

        [SetUp]
        public void Setup()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _messageService = new MessageService(_mockMessageRepository.Object);
        }

        [Test]
        public async Task CreateMessageAsync_ValidArguments_CreatesMessage()
        {
            // Arrange
            string senderId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a";
            string receiverId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81";
            string messageString = "Hello, World!";
            int clipId = 123;
            Message expectedMessage = new()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = messageString,
                ClipId = clipId,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            _mockMessageRepository.Setup(repo => repo.CreateMessageAsync(It.IsAny<Message>())).ReturnsAsync(expectedMessage);

            // Act
            Message result = await _messageService.CreateMessageAsync(senderId, receiverId, messageString, clipId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.SenderId, Is.EqualTo(senderId));
                Assert.That(result.ReceiverId, Is.EqualTo(receiverId));
                Assert.That(result.Content, Is.EqualTo(messageString));
                Assert.That(result.ClipId, Is.EqualTo(clipId));
            });
        }

        [Test]
        public void CreateMessageAsync_NullOrEmptyArguments_ThrowsArgumentException()
        {
            // Assert & Act
            Assert.ThrowsAsync<ArgumentException>(async () => await _messageService.CreateMessageAsync(null, "3d27ac2e-9b58-4b81-8c5d-6f470e496a81", "message", 123));
            Assert.ThrowsAsync<ArgumentException>(async () => await _messageService.CreateMessageAsync("1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a", null, "message", 123));
            Assert.ThrowsAsync<ArgumentException>(async () => await _messageService.CreateMessageAsync("1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a", "3d27ac2e-9b58-4b81-8c5d-6f470e496a81", null, 123));
        }

        [Test]
        public async Task GetMessagesAsync_ValidArguments_ReturnsMessages()
        {
            // Arrange
            string userId = "1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a";
            string friendId = "3d27ac2e-9b58-4b81-8c5d-6f470e496a81";
            List<Message> expectedMessages =
            [
                new() { SenderId = userId, ReceiverId = friendId, Content = "Hello" },
                new() { SenderId = friendId, ReceiverId = userId, Content = "Hi there" }
            ];

            _mockMessageRepository.Setup(repo => repo.GetMessagesAsync(userId, friendId)).ReturnsAsync(expectedMessages);

            // Act
            List<Message> result = await _messageService.GetMessagesAsync(userId, friendId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Has.Count.EqualTo(expectedMessages.Count));
                Assert.That(result[0].Content, Is.EqualTo(expectedMessages[0].Content));
                Assert.That(result[1].Content, Is.EqualTo(expectedMessages[1].Content));
            });
        }

        [Test]
        public void GetMessagesAsync_NullOrEmptyArguments_ThrowsArgumentException()
        {
            // Assert & Act
            Assert.ThrowsAsync<ArgumentException>(async () => await _messageService.GetMessagesAsync(null, "3d27ac2e-9b58-4b81-8c5d-6f470e496a81"));
            Assert.ThrowsAsync<ArgumentException>(async () => await _messageService.GetMessagesAsync("1f2e8b7c-0e13-4aae-b5b8-5d9830672f9a", null));
            Assert.ThrowsAsync<ArgumentException>(async () => await _messageService.GetMessagesAsync(null, null));
        }
    }
}
