using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spa.Domain.Entities;

namespace Spa.Infrastructure.EntityConfigurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {

            builder.HasKey(e => e.AppointmentID);
            builder.Property(e => e.AppointmentID).ValueGeneratedOnAdd(); //tự tăng


            //one to many with branch
            builder.HasOne(x => x.Branch)
           .WithMany(x => x.Appointments)
           .HasForeignKey(x => x.BranchID)
           .OnDelete(DeleteBehavior.Restrict);

            //one to many with Customer
            builder.HasOne(x => x.Customer)
            .WithMany(x => x.Appointments)
           .HasForeignKey(x => x.CustomerID)
           .OnDelete(DeleteBehavior.Restrict);




        }
    }
}
