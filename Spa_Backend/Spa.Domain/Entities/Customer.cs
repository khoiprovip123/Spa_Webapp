using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class Customer
    {
        public long CustomerID { get; set; }
        public string FirstName { get; set; }
        public string CustomerCode { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public int CustomerTypeID { get; set; } = 1;

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<TreatmentCard> TreatmentCards { get; set; }
        public ICollection<Sale> Sales { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
