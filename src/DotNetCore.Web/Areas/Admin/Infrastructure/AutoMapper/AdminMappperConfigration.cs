using System;
using AutoMapper;
using DotNetCore.Core.Interface;
using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Web.Areas.Admin.Models.Setting;
using DotNetCore.Core.Domain.Messages;
using DotNetCore.Web.Areas.Admin.Models.EmailAccounts;
using DotNetCore.Web.Areas.Admin.Models.Accounts;

namespace DotNetCore.Web.Areas.Admin.Infrastructure.AutoMapper
{
    public class AdminMappperConfigration : IMapperConfiguration
    {
        public int Order => 5;

        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = x =>
            {
                // Authorize Settings
                x.CreateMap<AuthorizeSettings, AuthorizeSettingsModel>();
                x.CreateMap<AuthorizeSettingsModel, AuthorizeSettings>();

                //Email account
                x.CreateMap<EmailAccount, EmailAccountModel>();
                x.CreateMap<EmailAccountModel, EmailAccount>();


                //Account
                x.CreateMap<Account, AccountModel>();
                x.CreateMap<AccountModel, Account>();
            };

            return action;
        }
    }
}
