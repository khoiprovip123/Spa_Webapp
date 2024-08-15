using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.IService
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessagesAsync();

        Task<bool> AddMessagesAsync(string user, string message);
    }
}
