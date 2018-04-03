using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Core.Domain.Messages;
using DotNetCore.Core.Infrastructure;
using DotNetCore.Web.Areas.Admin.Models.Accounts;
using DotNetCore.Web.Areas.Admin.Models.EmailAccounts;
using DotNetCore.Web.Areas.Admin.Models.Setting;

namespace DotNetCore.Web.Areas.Admin.Infrastructure.AutoMapper
{
    public static class MappingExtensions
    {
        #region Base

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }
        #endregion

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

        #region Email accounts

        public static EmailAccountModel ToModel(this EmailAccount entity)
        {
            return entity.MapTo<EmailAccount, EmailAccountModel>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model)
        {
            return model.MapTo<EmailAccountModel, EmailAccount>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model, EmailAccount destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Account

        public static AccountModel ToModel(this Account entity)
        {
            return entity.MapTo<Account, AccountModel>();
        }

        public static Account ToEntity(this AccountModel model)
        {
            return model.MapTo<AccountModel, Account>();
        }

        public static Account ToEntity(this AccountModel model, Account destination)
        {
            return model.MapTo(destination);
        }

        #endregion
    }
}
