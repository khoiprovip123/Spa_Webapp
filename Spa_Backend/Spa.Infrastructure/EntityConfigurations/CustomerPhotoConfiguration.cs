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
    internal class CustomerPhotoConfiguration : IEntityTypeConfiguration<CustomerPhoto>
    {
        public void Configure(EntityTypeBuilder<CustomerPhoto> builder)
        {
            builder.HasKey(e => e.PhotoID);
            builder.Property(e => e.PhotoID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.Appointments)
                    .WithMany(e => e.CustomerPhotos)
                    .HasForeignKey(e => e.AppointmentID)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
