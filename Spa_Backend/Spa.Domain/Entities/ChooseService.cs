using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
  
    public class ChooseService
    {
 
        public long AppointmentID { get; set; }
        public long ServiceID { get; set; }
      
        
        //ket noi 2 bang many to many
        public Appointment Appointment { get; set; }  
        public ServiceEntity Service { get; set; }
    }

}
