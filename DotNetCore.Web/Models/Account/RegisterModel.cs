﻿using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Web.Models.Account
{
    public class RegisterModel
    {
        [Required, MaxLength(256)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
