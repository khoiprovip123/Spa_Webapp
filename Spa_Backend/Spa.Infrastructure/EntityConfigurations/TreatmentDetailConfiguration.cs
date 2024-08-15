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
    internal class TreatmentDetailConfiguration : IEntityTypeConfiguration<TreatmentDetail>
    {
        public void Configure(EntityTypeBuilder<TreatmentDetail> builder)
        {
            builder.HasKey(e => e.TreatmentDetailID);
            builder.Property(e => e.TreatmentDetailID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.TreatmentCard)
                .WithMany(e => e.TreatmentDetails)
                .HasForeignKey(e => e.TreatmentID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Service)
              .WithMany(e => e.TreatmentDetails)
              .HasForeignKey(e => e.ServiceID)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
