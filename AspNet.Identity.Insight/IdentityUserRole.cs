﻿namespace AspNet.Identity.Insight
{
    /// <summary>
    /// Type that represents a user belonging to a role
    /// 
    /// </summary>
    public class IdentityUserRole : IdentityUserRole<string>
    {
    }

    /// <summary>
    /// Type that represents a user belonging to a role
    /// 
    /// </summary>
    /// <typeparam name="TKey"/>
    public class IdentityUserRole<TKey>
    {
        /// <summary>
        /// UserId for the user that is in the role
        /// 
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// RoleId for the role
        /// 
        /// </summary>
        public virtual TKey RoleId { get; set; }
    }
}
