using Microsoft.AspNetCore.Identity;
using MyProClip.Models;
using MyProClip_BLL.Models;

namespace MyProClip.Services
{
    public class MessageTransformer
    {
        private readonly ClipTransformer _clipTransformer = new();

        public List<MessageViewModel> MessagesToViewModel(List<Message> messages)
        {
            List<MessageViewModel> messageViewModels = [];

            foreach (Message message in messages)
            {
                MessageViewModel newMessage = new()
                {
                    Id = message.Id,
                    SenderId = message.SenderId,
                    Sender = message.Sender,
                    ReceiverId = message.ReceiverId,
                    ClipId = message.ClipId,
                    Clip = _clipTransformer.ClipToViewModel(message.Clip),
                    Content = message.Content,
                    UpdatedAt = message.UpdatedAt,
                    CreatedAt = message.CreatedAt,
                };

                messageViewModels.Add(newMessage);
            }

            return messageViewModels;     
        }
    }
}
