﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.Insight
{
    /// <summary>
    /// Type that represents one specific user claim
    /// 
    /// </summary>
    public class IdentityUserClaim : IdentityUserClaim<string>
    {
    }

    /// <summary>
    /// Type that represents one specific user claim
    /// 
    /// </summary>
    /// <typeparam name="TKey"/>
    public class IdentityUserClaim<TKey>
    {
        /// <summary>
        /// Primary key
        /// 
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// User Id for the user who owns this login
        /// 
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// Claim type
        /// 
        /// </summary>
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Claim value
        /// 
        /// </summary>
        public virtual string ClaimValue { get; set; }
    }
}
