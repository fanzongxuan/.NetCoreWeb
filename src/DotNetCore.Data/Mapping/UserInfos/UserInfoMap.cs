using DotNetCore.Core.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetCore.Data.Mapping.UserInfos
{
    public class UserInfoMap : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.HasQueryFilter(x => !x.IsDeleted).
                Property(x=>x.LoginName).HasMaxLength(50);
            builder.Property(x => x.Password).HasMaxLength(50);
        }
    }
}
