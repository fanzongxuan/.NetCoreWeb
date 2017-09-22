using DotNetCore.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Web.Areas.Admin.Models.EmailAccounts
{
    public class EmailAccountModel : BaseEntityModel
    {
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Host")]
        public string Host { get; set; }

        [Display(Name = "Port")]
        public int Port { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Enable Ssl")]
        public bool EnableSsl { get; set; }

        [Display(Name = "Use Default Credentials")]
        public bool UseDefaultCredentials { get; set; }

        [Display(Name = "Is Default Email Account")]
        public bool IsDefaultEmailAccount { get; set; }

        [Display(Name = "Send Test Email To")]
        public string SendTestEmailTo { get; set; }
    }
}
