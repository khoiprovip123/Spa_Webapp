using Microsoft.EntityFrameworkCore;
using Spa.Domain.Entities;
using Spa.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure
{
    public class MessageRepository : IMessageRepository
    {
        private readonly SpaDbContext _spaDbContext;

        public MessageRepository(SpaDbContext spaDbContext)
        {
            _spaDbContext = spaDbContext;
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            var messes = await _spaDbContext.Messages.ToListAsync();
            return messes;
        }

        public async Task<bool> AddMessagesAsync(Message message)
        {
            var newMess = new Message()
            {
                UserName = message.UserName,
                Content = message.Content,
                MessageTime = message.MessageTime,
            };
            try
            {
                await _spaDbContext.Messages.AddAsync(newMess);
                await _spaDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
            return false;
            }
        }

    }
}
