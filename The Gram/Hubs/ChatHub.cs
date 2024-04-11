using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using The_Gram.Data;
using The_Gram.Models;
using The_Gram.Services;

namespace The_Gram.Hubs
    {
        public class ChatHub : Hub
        {
            private readonly IChatService _chatService;

            public ChatHub(IChatService chatService)
            {
                _chatService = chatService;
            }

            public async Task SendMessage(int senderId, int receiverId, string message)
            {
                await _chatService.SendMessage(senderId, receiverId, message);

                await Clients.All.SendAsync("ReceiveMessage", senderId, receiverId, message);
            }
        }
    }


