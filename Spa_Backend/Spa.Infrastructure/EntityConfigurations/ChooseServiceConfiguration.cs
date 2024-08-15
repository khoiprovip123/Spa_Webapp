using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure.EntityConfigurations
{
    internal class ChooseServiceConfiguration : IEntityTypeConfiguration<ChooseService>
    {
        public void Configure(EntityTypeBuilder<ChooseService> builder)
        {
           
            builder.HasKey(sc => new { sc.ServiceID, sc.AppointmentID });


            builder.HasOne<Appointment>(e => e.Appointment)
                .WithMany(e => e.ChooseServices)
                .HasForeignKey(e => e.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ServiceEntity>(e => e.Service)
                .WithMany(e => e.ChooseServices)
                .HasForeignKey(e => e.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
