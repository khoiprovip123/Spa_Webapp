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
    internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {

            builder.HasKey(sc => new { sc.PermissionID, sc.JobTypeID });


            builder.HasOne<Permission>(e => e.Permissions)
                .WithMany(e => e.RolePermissions)
                .HasForeignKey(e => e.PermissionID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<JobType>(e => e.JobTypes)
                .WithMany(e => e.RolePermission)
                .HasForeignKey(e => e.JobTypeID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
