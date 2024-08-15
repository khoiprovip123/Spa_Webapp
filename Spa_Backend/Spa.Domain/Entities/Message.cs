using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Message
    {
        public int MessageId { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public DateTime MessageTime { get; set; }
    }
}
