namespace Spa.Domain.Entities
{
    public class CustomerPhoto
    {
        public long PhotoID { get; set; }
        public string PhotoPath { get; set; }
        public long AppointmentID { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public Appointment Appointments { get; set; }
    }
}