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
    public class BillItemConfiguration : IEntityTypeConfiguration<BillItem>
    {
        public void Configure(EntityTypeBuilder<BillItem> builder)
        {
            builder.HasKey(b => b.BillItemID);
            builder.Property(e => e.BillItemID).ValueGeneratedOnAdd();

            builder.HasOne(b => b.Bill)
                   .WithMany(i => i.BillItems)
                   .HasForeignKey(i => i.BillID);
                  
        }
    }
}
