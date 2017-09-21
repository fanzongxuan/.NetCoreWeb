using DotNetCore.Core.Domain.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Service.Messages
{
    public interface IEmailAccountService:IBaseService<EmailAccount>
    {
        IList<EmailAccount> GetAllEmailAccounts();
    }
}
