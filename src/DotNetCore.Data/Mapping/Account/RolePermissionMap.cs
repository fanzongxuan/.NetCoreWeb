using DotNetCore.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetCore.Data.Mapping.Account
{
    public class RolePermissionMap : IEntityTypeConfiguration<Core.Domain.Accounts.RolePermissionMap>
    {
        public void Configure(EntityTypeBuilder<Core.Domain.Accounts.RolePermissionMap> builder)
        {
            builder.Ignore(x => x.Id);

            builder.HasKey(x => new { x.AccountRoleId, x.PermissionRecordId });

            builder.HasOne(x => x.AccountRole)
                   .WithMany(x => x.RolePermissionMaps)
                   .HasForeignKey(x => x.AccountRoleId);

            builder.HasOne(x => x.PermissionRecord)
                   .WithMany(x => x.RolePermissionMaps)
                   .HasForeignKey(x => x.PermissionRecordId);
        }
    }
}
