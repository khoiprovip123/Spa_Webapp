namespace Spa.Domain.Entities
{
    public class JobType
    {
        public long JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public ICollection<RolePermission>? RolePermission { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Admin> Admins { get; set; }
    }
}