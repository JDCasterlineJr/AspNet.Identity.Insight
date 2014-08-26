using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using AspNet.Identity.Insight.Repositories.Interfaces;
using Insight.Database;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Insight.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _database;

        public UserRepository(IDbConnection database)
        {
            _database = database;
        }

        public UserRepository()
            : this(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Connection())
        {
        }

        public void InsertUser(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var i = _database.As<IUserRepository>();

            _database.Execute("InsertUser", new
            {
                email = user.Email,
                emailConfirmed = user.EmailConfirmed,
                passwordHash = user.PasswordHash,
                securityStamp = user.SecurityStamp,
                phoneNumber = user.PhoneNumber,
                phoneNumberConfirmed = user.PhoneNumberConfirmed,
                twoFactorEnabled = user.TwoFactorEnabled,
                lockoutEndDate = user.LockoutEndDate,
                lockoutEnabled = user.LockoutEnabled,
                accessFailedCount = user.AccessFailedCount,
                id = user.Id,
                userName = user.UserName
            });

            i.InsertUser(user);
        }

        public void UpdateUser(IdentityUser user)
        {
            _database.ExecuteSql(
                "UPDATE AspNetUsers SET Email = @email ,EmailConfirmed = @emailConfirmed ,PasswordHash = @passwordHash ,SecurityStamp = @securityStamp,PhoneNumber = @phoneNumber,PhoneNumberConfirmed = @phoneNumberConfirmed,TwoFactorEnabled = @twoFactorEnabled,LockoutEndDate = @lockoutEndDate,LockoutEnabled = @lockoutEnabled,AccessFailedCount = @accessFailedCount WHERE Id=@id",
                new
                {
                    email=user.Email,
                    emailConfirmed = user.EmailConfirmed,
                    passwordHash = user.PasswordHash,
                    securityStamp = user.SecurityStamp,
                    phoneNumber=user.PhoneNumber,
                    phoneNumberConfirmed = user.PhoneNumberConfirmed,
                    twoFactorEnabled = user.TwoFactorEnabled,
                    lockoutEndDate = user.LockoutEndDate,
                    lockoutEnabled = user.LockoutEnabled,
                    accessFailedCount = user.AccessFailedCount,
                    id = user.Id
                });
        }

        public void DeleteUser(string userId)
        {
            _database.ExecuteSql("DELETE FROM AspNetUsers WHERE Id=@userId", new { userId });
        }

        public IEnumerable<IdentityUser> GetAllUsers()
        {
            var resultSets = _database.QueryResultsSql<IdentityUser, FastExpando, FastExpando, FastExpando>(
                "SELECT * FROM AspNetUsers; " +
                "SELECT AspNetUsers.Id, AspNetUserClaims.* FROM AspNetUserClaims INNER JOIN AspNetUsers ON AspNetUserClaims.UserId = AspNetUsers.Id; " +
                "SELECT AspNetUsers.Id, AspNetUserLogins.* FROM AspNetUserLogins INNER JOIN AspNetUsers ON AspNetUserLogins.UserId = AspNetUsers.Id; " +
                "SELECT AspNetUsers.Id AS UserId, AspNetRoles.Name FROM AspNetUserRoles INNER JOIN AspNetUsers ON AspNetUserRoles.UserId = AspNetUsers.Id INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId;");

            var users = resultSets.Set1;
            var userclaims = resultSets.Set2;
            var userLogins = resultSets.Set3;
            var userRoles = resultSets.Set4;
            foreach (var user in users)
            {
                user.Claims.AddRange(
                    userclaims.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new Claim(x["ClaimType"].ToString(), x["ClaimValue"].ToString()))
                        .ToList());
                user.Logins.AddRange(
                    userLogins.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new UserLoginInfo(x["LoginProvider"].ToString(), x["ProviderKey"].ToString()))
                        .ToList());
                user.Roles.AddRange(
                    userRoles.Where(x => x["UserId"].ToString() == user.Id).Select(x => x["Name"].ToString()));
            }
            return users;
        }
        public IdentityUser GetUserById(string userId)
        {
            var resultSets = _database.QueryResultsSql<IdentityUser, FastExpando, FastExpando, FastExpando>(
                "SELECT * FROM AspNetUsers WHERE Id = @userId; " +
                "SELECT AspNetUsers.Id, AspNetUserClaims.* FROM AspNetUserClaims INNER JOIN AspNetUsers ON AspNetUserClaims.UserId = AspNetUsers.Id WHERE AspNetUsers.Id = @userId; " +
                "SELECT AspNetUsers.Id, AspNetUserLogins.* FROM AspNetUserLogins INNER JOIN AspNetUsers ON AspNetUserLogins.UserId = AspNetUsers.Id WHERE AspNetUsers.Id = @userId; " +
                "SELECT AspNetUsers.Id AS UserId, AspNetRoles.Name FROM AspNetUserRoles INNER JOIN AspNetUsers ON AspNetUserRoles.UserId = AspNetUsers.Id INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId WHERE AspNetUsers.Id = @userId;",
                new { userId });

            var users = resultSets.Set1;
            var userclaims = resultSets.Set2;
            var userLogins = resultSets.Set3;
            var userRoles = resultSets.Set4;
            foreach (var user in users)
            {
                user.Claims.AddRange(
                    userclaims.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new Claim(x["ClaimType"].ToString(), x["ClaimValue"].ToString()))
                        .ToList());
                user.Logins.AddRange(
                    userLogins.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new UserLoginInfo(x["LoginProvider"].ToString(), x["ProviderKey"].ToString()))
                        .ToList());
                user.Roles.AddRange(
                    userRoles.Where(x => x["UserId"].ToString() == user.Id).Select(x => x["Name"].ToString()));
            }
            return users.SingleOrDefault();
        }

        public IEnumerable<IdentityUser> GetUserByName(string userName)
        {
            var resultSets = _database.QueryResultsSql<IdentityUser, FastExpando, FastExpando, FastExpando>(
                "SELECT * FROM AspNetUsers WHERE UserName = @userName; " +
                "SELECT AspNetUsers.Id, AspNetUserClaims.* FROM AspNetUserClaims INNER JOIN AspNetUsers ON AspNetUserClaims.UserId = AspNetUsers.Id WHERE AspNetUsers.UserName = @userName; " +
                "SELECT AspNetUsers.Id, AspNetUserLogins.* FROM AspNetUserLogins INNER JOIN AspNetUsers ON AspNetUserLogins.UserId = AspNetUsers.Id WHERE AspNetUsers.UserName = @userName; " +
                "SELECT AspNetUsers.Id AS UserId, AspNetRoles.Name FROM AspNetUserRoles INNER JOIN AspNetUsers ON AspNetUserRoles.UserId = AspNetUsers.Id INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId WHERE AspNetUsers.UserName = @userName;",
                new {userName});

            var users = resultSets.Set1;
            var userclaims = resultSets.Set2;
            var userLogins = resultSets.Set3;
            var userRoles = resultSets.Set4;
            foreach (var user in users)
            {
                user.Claims.AddRange(
                    userclaims.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new Claim(x["ClaimType"].ToString(), x["ClaimValue"].ToString()))
                        .ToList());
                user.Logins.AddRange(
                    userLogins.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new UserLoginInfo(x["LoginProvider"].ToString(), x["ProviderKey"].ToString()))
                        .ToList());
                user.Roles.AddRange(
                    userRoles.Where(x => x["UserId"].ToString() == user.Id).Select(x => x["Name"].ToString()));
            }
            return users;
        }

        public IEnumerable<IdentityUser> GetUserByEmail(string email)
        {
            var resultSets = _database.QueryResultsSql<IdentityUser, FastExpando, FastExpando, FastExpando>(
                "SELECT * FROM AspNetUsers WHERE Email = @email; " +
                "SELECT AspNetUsers.Id, AspNetUserClaims.* FROM AspNetUserClaims INNER JOIN AspNetUsers ON AspNetUserClaims.UserId = AspNetUsers.Id WHERE AspNetUsers.Email = @email; " +
                "SELECT AspNetUsers.Id, AspNetUserLogins.* FROM AspNetUserLogins INNER JOIN AspNetUsers ON AspNetUserLogins.UserId = AspNetUsers.Id WHERE AspNetUsers.Email = @email; " +
                "SELECT AspNetUsers.Id AS UserId, AspNetRoles.Name FROM AspNetUserRoles INNER JOIN AspNetUsers ON AspNetUserRoles.UserId = AspNetUsers.Id INNER JOIN AspNetRoles ON AspNetRoles.Id = AspNetUserRoles.RoleId WHERE AspNetUsers.Email = @email;",
                new { email });

            var users = resultSets.Set1;
            var userclaims = resultSets.Set2;
            var userLogins = resultSets.Set3;
            var userRoles = resultSets.Set4;
            foreach (var user in users)
            {
                user.Claims.AddRange(
                    userclaims.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new Claim(x["ClaimType"].ToString(), x["ClaimValue"].ToString()))
                        .ToList());
                user.Logins.AddRange(
                    userLogins.Where(x => x["UserId"].ToString() == user.Id)
                        .Select(x => new UserLoginInfo(x["LoginProvider"].ToString(), x["ProviderKey"].ToString()))
                        .ToList());
                user.Roles.AddRange(
                    userRoles.Where(x => x["UserId"].ToString() == user.Id).Select(x => x["Name"].ToString()));
            }
            return users;
        }
    }
}
