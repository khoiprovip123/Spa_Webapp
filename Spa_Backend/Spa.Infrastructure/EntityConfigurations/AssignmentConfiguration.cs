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
    internal class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {

            builder.HasKey(sc => new { sc.EmployerID, sc.AppointmentID });


            builder.HasOne<Appointment>(e => e.Appointment)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Employee>(e => e.Employees)
                .WithMany(e => e.Assignments)
                .HasForeignKey(e => e.EmployerID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
