using The_Gram.Data.Models;

namespace The_Gram.Services
{
    public interface IChatService
    {

        public Task SendMessage(int senderId, int receiverId, string message);
        public Task<List<Message>> GetMessagesForUser(int receiverId);
    }
}
