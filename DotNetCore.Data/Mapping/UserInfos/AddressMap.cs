using DotNetCore.Core.Domain.UserInfos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetCore.Data.Mapping.UserInfos
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
