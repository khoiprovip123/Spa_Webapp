using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<bool> AddMessagesAsync(string user, string message)
        {
            Message mess = new Message 
            { 
                UserName = user,
                Content = message,
                MessageTime = DateTime.Now,
            };
          return await _messageRepository.AddMessagesAsync(mess);
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
        return  await _messageRepository.GetMessagesAsync();
        }
    }
}
