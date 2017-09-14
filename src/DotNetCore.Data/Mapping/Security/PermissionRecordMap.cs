using DotNetCore.Core.Domain.ScheduleTasks;
using DotNetCore.Core.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Data.Mapping.ScheduleTasks
{
    public class PermissionRecordMap : IEntityTypeConfiguration<PermissionRecord>
    {
        public void Configure(EntityTypeBuilder<PermissionRecord> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.SystemName).IsRequired();
            
        }
    }
}
