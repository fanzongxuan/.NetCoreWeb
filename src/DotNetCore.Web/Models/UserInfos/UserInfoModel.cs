using DotNetCore.Core.Domain.Accounts;
using DotNetCore.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Web.Models.UserInfos
{
    public class UserInfoModel : BaseModel
    {
        [Required(ErrorMessage = "名称不能为空")]
        [MinLength(5, ErrorMessage = "长度不能小于5"), MaxLength(20, ErrorMessage = "长度不能大于20")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(5, ErrorMessage = "长度不能小于5"), MaxLength(20, ErrorMessage = "长度不能大于20")]
        public string Password { get; set; }

        public string RealName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        public string LastLoginIpAddress { get; set; }

        public Sex Sex { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
        
        public virtual List<AddressModel> Addresses { get; set; }
    }
}
