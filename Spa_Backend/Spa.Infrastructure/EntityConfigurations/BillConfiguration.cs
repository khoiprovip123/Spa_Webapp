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
    public class BillConfiguration :IEntityTypeConfiguration<Bill>
    {

        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(b => b.BillID);
            builder.Property(e => e.BillID).ValueGeneratedOnAdd();
           

            // Relationship
             builder.HasOne(p => p.Appointment)
            .WithOne(a => a.Bill)
            .HasForeignKey<Bill>(p => p.AppointmentID);

         
        }
    }
}
