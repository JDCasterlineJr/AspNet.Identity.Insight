using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AspNet.Identity.Insight.Repositories.Interfaces;
using Insight.Database;

namespace AspNet.Identity.Insight.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IDbConnection _database;

        public RoleRepository(IDbConnection database)
        {
            _database = database;
        }

        public RoleRepository()
            : this(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Connection())
        {
        }

        public void InsertRole(IdentityRole role)
        {
            _database.ExecuteSql(
                "INSERT INTO AspNetRoles (Id, Name) VALUES (@id, @name)", new { id = role.Id, name = role.Name });
        }

        public void UpdateRole(IdentityRole role)
        {
            _database.ExecuteSql(
                "UPDATE AspNetRoles SET Name = @name WHERE Id = @id", new { id = role.Id, name = role.Name });
        }

        public void DeleteRole(string roleId)
        {
            _database.ExecuteSql(
                "DELETE FROM AspNetRoles WHERE Id = @id", new {id = roleId});
        }

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _database.QuerySql<IdentityRole>("SELECT * FROM AspNetRoles");
        }
        public string GetRoleName(string roleId)
        {
            return _database.SingleSql<string>(
                "SELECT Name FROM AspNetRoles WHERE Id = @id", new {id = roleId});
        }

        public string GetRoleId(string roleName)
        {
            return _database.SingleSql<string>(
                "SELECT Id FROM AspNetRoles WHERE Name = @name", new { name = roleName });
        }

        public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);

            return roleName != null ? new IdentityRole(roleName, roleId) : null;
        }

        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);

            return roleId != null ? new IdentityRole(roleName, roleId) : null;
        }
    }
}
