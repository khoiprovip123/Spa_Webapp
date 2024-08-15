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
    internal class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasKey(e => e.AdminID);
            builder.Property(e => e.AdminID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.JobType)
                    .WithMany(e => e.Admins)
                    .HasForeignKey(e => e.JobTypeID)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
