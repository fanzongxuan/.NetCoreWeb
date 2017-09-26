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
        [Display(Name = "Email"), Required]
        public string Email { get; set; }

        [Display(Name = "Display Name"), Required]
        public string DisplayName { get; set; }

        [Display(Name = "Host"), Required]
        public string Host { get; set; }

        [Display(Name = "Port"), Required]
        public int Port { get; set; }

        [Display(Name = "Username"), Required]
        public string Username { get; set; }

        [Display(Name = "Password"), Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

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
