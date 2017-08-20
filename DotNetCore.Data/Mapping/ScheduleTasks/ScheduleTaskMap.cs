using DotNetCore.Core.Domain.ScheduleTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Data.Mapping.ScheduleTasks
{
    class ScheduleTaskMap : IEntityTypeConfiguration<ScheduleTask>
    {
        public void Configure(EntityTypeBuilder<ScheduleTask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Type).IsRequired();
        }
    }
}
