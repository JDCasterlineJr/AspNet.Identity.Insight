using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Insight
{
    public class IdentityUser : IUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUser"/> class.
        /// </summary>
        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();
            Claims = new List<Claim>();
            Roles = new List<string>();
            Logins = new List<UserLoginInfo>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUser"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public IdentityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        /// <summary>
        /// User ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User's name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        ///     The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        ///     A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        ///     DateTimeOffset when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTimeOffset? LockoutEndDate { get; set; }

        /// <summary>
        ///     Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public virtual List<string> Roles { get; set; }

        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <value>The claims.</value>
        public virtual List<Claim> Claims { get; set; }

        /// <summary>
        /// Gets the logins.
        /// </summary>
        /// <value>The logins.</value>
        public virtual List<UserLoginInfo> Logins { get; set; }
    }
}
