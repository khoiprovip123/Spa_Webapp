namespace Spa.Domain.Entities
{
    public class Employee
    {
        public long? EmployeeID { get; set; }
        public string? Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public long? JobTypeID { get; set; }
        public long? BranchID { get; set; }
          
        public JobType? JobType { get; set; }
        public Branch? Branch { get; set; }

        public bool IsActive { get; set; } = true;
        public ICollection<Sale>? Sales { get; set; }

        public ICollection<Assignment>? Assignments { get; set; }

        public ICollection<User>? User { get; set; }
    }
}