namespace Spa.Domain.Entities
{
    public class Assignment
    {
        public long EmployerID { get; set; }
        public long AppointmentID { get; set; }

        public Appointment Appointment { get; set; }
        public Employee Employees { get; set; }
    }
}