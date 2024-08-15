using Microsoft.AspNetCore.Identity;
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
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Email).IsRequired();
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Role).IsRequired();
            builder.HasIndex(u => u.EmployeeID).IsUnique();

            builder.HasIndex(u => u.AdminID).IsUnique();

            builder.HasOne(u => u.Employee)
                 .WithMany(e => e.User)
                 .HasForeignKey(u => u.EmployeeID)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Admin)
              .WithMany(e => e.User)
              .HasForeignKey(u => u.AdminID)
              .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
