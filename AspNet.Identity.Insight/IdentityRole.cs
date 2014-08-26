using System;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Insight
{
    public class IdentityRole : IRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRole"/> class.
        /// </summary>
        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRole"/> class.
        /// </summary>
        /// <param name="name">Name of the role.</param>
        public IdentityRole(string name)
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityRole"/> class.
        /// </summary>
        /// <param name="name">Name of the role.</param>
        /// <param name="id">Id of the role.</param>
        public IdentityRole(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// Role ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }
    }
}
