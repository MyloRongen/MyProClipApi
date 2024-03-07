using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MyProClip.Models;
using MyProClip.Services;
using MyProClip_BLL.Interfaces.Services;
using MyProClip_BLL.Models;
using MyProClip_BLL.Services;
using System.Reflection;
using System.Security.Claims;

namespace MyProClip.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly MessageTransformer _messageTransformer = new();
        private readonly ClipTransformer _clipTransformer = new();
        private static readonly List<UserConnection> _userConnections = [];
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IClipService _clipService;
        private readonly UserManager<IdentityUser> _userManager;

        public ChatHub(IFriendshipService friendshipService, IUserService userService, UserManager<IdentityUser> userManager, IMessageService messageService, IClipService clipService)
        {
            _friendshipService = friendshipService;
            _userService = userService;
            _userManager = userManager;
            _messageService = messageService;
            _clipService = clipService;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Task.CompletedTask;
            }

            CloseUserConnections(userId);

            _userConnections.Add(new UserConnection { UserId = userId, ConnectionId = Context?.ConnectionId });
            return base.OnConnectedAsync();
        }

        private async void CloseUserConnections(string userId)
        {
            var connectionsToRemove = _userConnections.Where(c => c.UserId == userId).ToList();
            foreach (var connectionToRemove in connectionsToRemove)
            {
                _userConnections.Remove(connectionToRemove);
                await Clients.Client(connectionToRemove.ConnectionId).SendAsync("CloseConnection");
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connectionToRemove = _userConnections.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);
            if (connectionToRemove != null)
            {
                _userConnections.Remove(connectionToRemove);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task GetMessages(string receiverName)
        {
            try
            {
                string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                IdentityUser? receiver = await _userService.FindUserByNameAsync(receiverName) ?? throw new Exception("Friend not found.");
                IdentityUser? user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");

                bool friendshipExists = await _friendshipService.FriendshipExists(receiver.Id, user.Id);
                if (!friendshipExists)
                {
                    throw new Exception("Friendship doesn't exist.");
                }

                List<Message> messages = await _messageService.GetMessagesAsync(userId, receiver.Id);
                List<MessageViewModel> messageViewModels = _messageTransformer.MessagesToViewModel(messages);

                messageViewModels = [.. messageViewModels.OrderBy(m => m.UpdatedAt)];

                UserConnection? userConnection = _userConnections.FirstOrDefault(c => c.UserId == user.Id);
                if (userConnection != null)
                {
                    foreach (var message in messageViewModels)
                    {
                        await Clients.Client(userConnection.ConnectionId).SendAsync("ReceiveMessage", message.Sender.UserName, message.Content, message.Clip);
                    }
                }
            }
            catch (Exception)
            {
                // return exception
            }
        }


        public async Task SendMessage(string receiverName, string message, int clipId)
        {
            try
            {
                string? userId = Context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                IdentityUser? receiver = await _userService.FindUserByNameAsync(receiverName) ?? throw new Exception("Friend not found.");          
                IdentityUser? sender = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
                
                bool friendshipExists = await _friendshipService.FriendshipExists(receiver.Id, sender.Id);
                if (!friendshipExists)
                {
                    throw new Exception("Friendship doesn't exist.");
                }

                ClipViewModel? clipViewModel = null;

                if (clipId > 0)
                {
                    Clip? clip = await _clipService.GetClipById(clipId);
                    clipViewModel = _clipTransformer.ClipToViewModel(clip);
                }

                UserConnection? receiverConnection = _userConnections.FirstOrDefault(c => c.UserId == receiver.Id);
                if (receiverConnection != null)
                {
                    await Clients.Client(receiverConnection.ConnectionId).SendAsync("ReceiveMessage", sender.UserName, message, clipViewModel);
                }

                UserConnection? senderConnection = _userConnections.FirstOrDefault(c => c.UserId == sender.Id);
                if (senderConnection != null)
                {
                    await Clients.Client(senderConnection.ConnectionId).SendAsync("ReceiveMessage", sender.UserName, message, clipViewModel);
                }

                var createdMessage = await _messageService.CreateMessageAsync(sender.Id, receiver.Id, message, clipId);
            }
            catch (Exception)
            {
                // return exception
            }
        }
    }
}
