using System.Collections.Generic;

namespace AspNet.Identity.Insight.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        void InsertRole(IdentityRole role);
        void UpdateRole(IdentityRole role);
        void DeleteRole(string roleId);
        IEnumerable<IdentityRole> GetAllRoles();
        string GetRoleName(string roleId);
        string GetRoleId(string roleName);
        IdentityRole GetRoleById(string roleId);
        IdentityRole GetRoleByName(string roleName);
    }
}