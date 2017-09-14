using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Web.Areas.Admin.Models.Setting;

namespace DotNetCore.Web.Areas.Admin.Infrastructure.AutoMapper
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Settings

        public static AuthorizeSettingsModel ToModel(this AuthorizeSettings entity)
        {
            return entity.MapTo<AuthorizeSettings, AuthorizeSettingsModel>();
        }

        public static AuthorizeSettings ToEntity(this AuthorizeSettingsModel model)
        {
            return model.MapTo<AuthorizeSettingsModel, AuthorizeSettings>();
        }

        #endregion
    }
}
