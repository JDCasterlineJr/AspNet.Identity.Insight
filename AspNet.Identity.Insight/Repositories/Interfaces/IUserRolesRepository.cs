using System.Collections.Generic;

namespace AspNet.Identity.Insight.Repositories.Interfaces
{
    public interface IUserRolesRepository
    {
        void InsertUserRole(string userId, string roleId);
        void DeleteUserRole(string userId, string roleId);
        IEnumerable<string> FindByUserId(string userId);
    }
}