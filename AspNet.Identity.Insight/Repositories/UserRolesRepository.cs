using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AspNet.Identity.Insight.Repositories.Interfaces;
using Insight.Database;

namespace AspNet.Identity.Insight.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        private readonly IDbConnection _database;

        public UserRolesRepository(IDbConnection database)
        {
            _database = database;
        }

        public UserRolesRepository()
            : this(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Connection())
        {
        }

        public void InsertUserRole(string userId, string roleId)
        {
            _database.ExecuteSql(
                "INSERT INTO AspNetUserRoles (UserId, RoleId) values (@userId, @roleId)",
                new {userId, roleId});
        }

        public void DeleteUserRole(string userId, string roleId)
        {
            _database.ExecuteSql(
               "DELETE FROM AspNetUserRoles WHERE UserId = @userId AND RoleId = @roleId",
               new {userId, roleId});
        }

        public IEnumerable<string> FindByUserId(string userId)
        {
            var result =_database.QuerySql<string>(
                "SELECT AspNetRoles.Name FROM AspNetUserRoles INNER JOIN AspNetRoles ON AspNetUserRoles.RoleId=AspNetRoles.Id WHERE AspNetUserRoles.UserId = @userId",
            new {userId});

            return result.Select(x=>x).ToList();
        }
    }
}
