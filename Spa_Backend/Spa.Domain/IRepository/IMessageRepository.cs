using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IRepository
{
    public interface IMessageRepository
    {
       Task<bool> AddMessagesAsync(Message message);

        Task<List<Message>> GetMessagesAsync();
    }
}
