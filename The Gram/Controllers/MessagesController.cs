using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using System.Threading.Tasks;
using The_Gram.Data;
using The_Gram.Data.Models;
using The_Gram.Models.Chat;
using The_Gram.Services;

namespace The_Gram.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        private readonly IChatService _chatService;
        private readonly ApplicationDbContext _dbContext;

        public MessagesController(IChatService chatService, ApplicationDbContext dbContext)
        {
            _chatService = chatService;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Chat()
        {
            List<The_Gram.Models.Chat.ChatMessageViewModel> messages = await _dbContext.Messages
                .Include(message => message.Receiver)
                .Select(message => new ChatMessageViewModel
                {
                    Content = message.Text,
                    SenderName = message.Receiver.Username
                })
                .ToListAsync();

            return View(messages);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage(int senderId, int receiverId, string message)
        {
            await _chatService.SendMessage(senderId, receiverId, message);
            return View(message);
        }
    }
}
