using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AspNet.Identity.Insight.Repositories.Interfaces;
using Insight.Database;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Insight.Repositories
{
    public class UserLoginsRepository : IUserLoginsRepository
    {
        private readonly IDbConnection _database;

        public UserLoginsRepository(IDbConnection database)
        {
            _database = database;
        }

        public UserLoginsRepository()
            : this(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Connection())
        {
        }

        public void InsertLogin(string userId, UserLoginInfo login)
        {
            _database.ExecuteSql(
                           "INSERT INTO AspNetUserLogins (UserId, LoginProvider, ProviderKey) VALUES (@userId, @loginProvider, @providerKey)",
                           new { userId, loginProvider = login.LoginProvider, providerKey = login.ProviderKey });
        }

        public void DeleteLogin(string userId, UserLoginInfo login)
        {
            _database.ExecuteSql(
                           "DELETE FROM AspNetUserLogins WHERE UserId = @userId AND LoginProvider = @loginProvider AND ProviderKey = @providerKey",
                           new { userId, loginProvider = login.LoginProvider, providerKey = login.ProviderKey });
        }

        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            return _database.SingleSql<string>(
               "SELECT UserId FROM AspNetUserLogins WHERE LoginProvider = @loginProvider AND ProviderKey = @providerKey",
               new { loginProvider = userLogin.LoginProvider, providerKey = userLogin.ProviderKey });
        }

        public IEnumerable<UserLoginInfo> FindLoginsByUserId(string userId)
        {
            var result = _database.QuerySql(
               "SELECT * FROM AspNetUserLogins WHERE UserId = @userId",
               new { userId });

            return result.Select(x => new UserLoginInfo(x["LoginProvider"].ToString(), x["ProviderKey"].ToString()))
                    .ToList();
        }
    }
}
