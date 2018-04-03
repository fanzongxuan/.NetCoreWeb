using DotNetCore.Web.Areas.Admin.Models.EmailAccounts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Web.Areas.Admin.Models.Accounts
{
    public class AccountModel
    {
        public AccountModel()
        {
            Roles = new List<string>();
            AvailaleRoles = new List<SelectListItem>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        [EmailAddress, Required]
        public string Email { get; set; }

        [Phone]
        public int PhoneNumber { get; set; }

        public IList<string> Roles { get; set; }

        public IList<SelectListItem> AvailaleRoles { get; set; }
    }
}
