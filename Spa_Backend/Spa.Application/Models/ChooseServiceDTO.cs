using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public class ChooseServiceDTO
    {
        public long AppointmentID { get; set; }
        public long ServiceID { get; set; }
        public ServiceDTO service { get; set; }
    }
}
