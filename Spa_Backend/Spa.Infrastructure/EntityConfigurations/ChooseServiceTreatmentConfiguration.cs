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
    internal class ChooseServiceTreatmentConfiguration : IEntityTypeConfiguration<ChooseServiceTreatment>
    {
 
        public void Configure(EntityTypeBuilder<ChooseServiceTreatment> builder)
        {
            builder.HasKey(sc => new { sc.TreatmentDetailID, sc.AppointmentID });


            builder.HasOne<Appointment>(e => e.Appointment)
                .WithMany(e => e.ChooseServiceTreatments)
                .HasForeignKey(e => e.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<TreatmentDetail>(e => e.TreatmentDetail)
                .WithMany(e => e.ChooseServiceTreatment)
                .HasForeignKey(e => e.TreatmentDetailID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
