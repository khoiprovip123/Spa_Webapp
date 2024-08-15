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
    public class TreatmentCardConfiguation : IEntityTypeConfiguration<TreatmentCard>
    {
        public void Configure(EntityTypeBuilder<TreatmentCard> builder)
        {
            builder.HasKey(e => e.TreatmentID);
            builder.Property(e => e.TreatmentID).ValueGeneratedOnAdd(); //tự tăng

            builder.HasOne(e => e.Customer)
                .WithMany(e => e.TreatmentCards)
                .HasForeignKey(e => e.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
