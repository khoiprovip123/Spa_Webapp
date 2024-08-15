using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Spa.Domain.Entities;
using Spa.Infrastructure.EntityConfigurations;

namespace Spa.Infrastructure
{
    public class SpaDbContext : DbContext
    {
        public SpaDbContext(DbContextOptions<SpaDbContext> options) : base(options)
        {
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<CustomerType> ClientTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<ChooseService> ChooseServices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CustomerPhoto> CustomerPhotos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<BillItem> BillItem { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<TreatmentCard> TreatmentCards { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<TreatmentDetail> TreatmentDetails { get; set; }
        public DbSet<IncomeExpenses> IncomeExpenses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdminConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new ChooseServiceConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new JobTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseConfiguration());
            modelBuilder.ApplyConfiguration(new SaleConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerPhotoConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new BillItemConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new TreatmentCardConfiguation());
            modelBuilder.ApplyConfiguration(new TreatmentDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ChooseServiceTreatmentConfiguration());
            modelBuilder.ApplyConfiguration(new IncomeExpensesConfiguration());

        }
    }

    public class BloggingContextFactory : IDesignTimeDbContextFactory<SpaDbContext>
    {
        public SpaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SpaDbContext>();
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-5ABH3PVT;Initial Catalog=SpaShop6;Persist Security Info=True;User ID=sa;Password=sa;Trust Server Certificate=True");

            return new SpaDbContext(optionsBuilder.Options);
        }
    }
}
