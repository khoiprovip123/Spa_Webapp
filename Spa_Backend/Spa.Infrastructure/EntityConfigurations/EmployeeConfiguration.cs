using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure.EntityConfigurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeID);
            builder.Property(e => e.EmployeeID).ValueGeneratedOnAdd();
         

            builder.HasOne(e => e.Branch)
                .WithMany(e => e.Employees)
                .HasForeignKey(e => e.BranchID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JobType)
               .WithMany(e => e.Employees)
               .HasForeignKey(e => e.JobTypeID)
               .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(u => u.User)
            //       .WithOne(e => e.Employee)
            //       .HasForeignKey(e=> e.)


        }
    }
}
