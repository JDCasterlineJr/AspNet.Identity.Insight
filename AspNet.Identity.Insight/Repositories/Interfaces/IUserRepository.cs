using System.Collections.Generic;

namespace AspNet.Identity.Insight.Repositories.Interfaces
{
    public interface IUserRepository
    {
        void InsertUser(IdentityUser user);
        void UpdateUser(IdentityUser user);
        void DeleteUser(string userId);
        IEnumerable<IdentityUser> GetAllUsers();
        IdentityUser GetUserById(string userId);
        IEnumerable<IdentityUser> GetUserByName(string userName);
        IEnumerable<IdentityUser> GetUserByEmail(string email);
    }
}
