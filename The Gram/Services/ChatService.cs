using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using The_Gram.Data;
using The_Gram.Data.Models;
using The_Gram.Hubs;

namespace The_Gram.Services
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public ChatService(IHubContext<ChatHub> hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task SendMessage(int senderId, int receiverId, string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", senderId, receiverId, message);
        }
        /*
        public async Task<List<Message>> GetMessagesForUser(int receiverId)
        {
            var messages = await _context.Messages
                .Where(m => m.ReceiverId == receiverId)
                .ToListAsync();

            return messages;
        }*/
        public async Task<List<The_Gram.Data.Models.Message>> GetMessagesForUser(int receiverId)
        {
            var messages = await _context.Messages
                .Where(m => m.ReceiverId == receiverId)
                .ToListAsync();

            return messages;
        }
    }
}
