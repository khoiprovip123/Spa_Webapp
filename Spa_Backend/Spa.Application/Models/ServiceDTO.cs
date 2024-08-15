using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class ServiceDTO
    {
        public long? ServiceID { get; set; }
        public string? ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
