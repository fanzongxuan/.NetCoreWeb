using System;

namespace DotNetCore.Web.Areas.Admin.Models.Setting
{
    public class AuthorizeSettingsModel
    {
        /// <summary>
        /// Reqire Unique Email
        /// </summary>
        public bool RequireUniqueEmail { get; set; }

        /// <summary>
        ///  Gets or sets the The minimum length of password
        /// </summary>
        public int RequirePasswordLength { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of unique chars a password must comprised of.
        /// </summary>
        public int RequiredPasswordUniqueChars { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a non-alphanumeric character.
        /// </summary>
        public bool RequirePasswordNonAlphanumeric { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a lower case ASCII character.
        /// </summary>
        public bool RequirePasswordLowercase { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a upper case ASCII character.
        /// </summary>
        public bool RequirePasswordUppercase { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a upper case ASCII character.
        /// </summary>
        public bool RequirePasswordDigit { get; set; }

        /// <summary>
        /// Gets or sets the number of failed access attempts allowed before a user is locked  
        /// out, assuming lock out is enabled.
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; }

        /// <summary>
        /// Gets or sets the System.TimeSpan a user is locked out for when a lockout occurs.
        /// </summary>
        public TimeSpan DefaultLockoutTimeSpan { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether a confirmed email address is required
        //  to sign in.
        /// </summary>
        public bool RequireConfirmedEmail { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether a confirmed telephone number is required
        // to sign in.
        /// </summary>
        public bool RequireConfirmedPhoneNumber { get; set; }
    }
}
