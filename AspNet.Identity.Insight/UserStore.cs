using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Identity.Insight.Repositories;
using Insight.Database;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Insight
{
    /// <summary>
    ///     Insight.Database based user store implementation that supports IUserStore, IUserLoginStore, IUserRoleStore, IUserClaimStore, IUserPasswordStore,
    ///     IUserSecurityStampStore, IUserTwoFactorStore, IUserLockoutStore, IUserEmailStore, IUserPhoneNumberStore, IQueryableUserStore
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class UserStore<TUser> :
        IUserStore<TUser>,
        IUserLoginStore<TUser>,
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserTwoFactorStore<TUser, string>,
        IUserLockoutStore<TUser, string>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IQueryableUserStore<TUser>
        where TUser : IdentityUser
    {
        private readonly RoleRepository _roleRepository;
        private readonly UserClaimsRepository _userClaimsRepository;
        private readonly UserLoginsRepository _userLoginsRepository;
        private readonly UserRepository _userRepository;
        private readonly UserRolesRepository _userRolesRepository;

        /// <summary>
        ///     Default constuctor which initializes a new Insight Database instance using the Default Connection string
        /// </summary>
        public UserStore()
            : this(
                new SqlConnectionStringBuilder(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Connection())
        {
        }

        /// <summary>
        ///     Constructor which takes a db connection and wires up the repositories with default instances using the database
        /// </summary>
        /// <param name="database"></param>
        public UserStore(IDbConnection database)
        {
            if (database == null)
                throw new ArgumentNullException("database");

            _userRepository = new UserRepository(database);
            _userClaimsRepository = new UserClaimsRepository(database);
            _userLoginsRepository = new UserLoginsRepository(database);
            _roleRepository = new RoleRepository(database);
            _userRolesRepository = new UserRolesRepository(database);
        }

        #region IQueryableUserStore<TUser> Members

        /// <summary>
        /// Get all Users in the AspNetUsers table
        /// </summary>
        public IQueryable<TUser> Users
        {
            get { return (IQueryable<TUser>) _userRepository.GetAllUsers().AsQueryable(); }
        }

        #endregion

        #region IUserClaimStore<TUser> Members

        /// <summary>
        /// Inserts an entry into the AspNetUserClaims table for the given TUser
        /// </summary>
        /// <param name="user">User to have claim added</param>
        /// <param name="claim">Claim to be added</param>
        /// <returns></returns>
        public Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (claim == null)
                throw new ArgumentNullException("claim");

            _userClaimsRepository.InsertClaim(user.Id, claim);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns all claims for the given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var claimsIdentity = _userClaimsRepository.FindClaimsByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(claimsIdentity.Claims.ToList());
        }

        /// <summary>
        /// Removes a claim from a user
        /// </summary>
        /// <param name="user">User to have claim removed</param>
        /// <param name="claim">Claim to be removed</param>
        /// <returns></returns>
        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (claim == null)
                throw new ArgumentNullException("claim");

            _userClaimsRepository.DeleteClaim(user.Id, claim);

            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserEmailStore<TUser> Members

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<TUser> FindByEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email))
                throw new ArgumentException("email");

            var result = _userRepository.GetUserById(email);

            return Task.FromResult((TUser) result);
        }

        /// <summary>
        /// Returns the Email for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetEmailAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.Email);
        }

        /// <summary>
        /// Returns EmailConfirmed for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.EmailConfirmed);
        }

        /// <summary>
        /// Sets Email for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task SetEmailAsync(TUser user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Email = email;

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Sets EmailConfirmed for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;

            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserLockoutStore<TUser,string> Members

        /// <summary>
        /// Returns the AccessFailedCount for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.AccessFailedCount);
        }

        /// <summary>
        /// Returns LockoutEnabled for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEnabled);
        }

        /// <summary>
        /// Returns the LockoutEndDate for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEndDate.GetValueOrDefault());
        }

        /// <summary>
        /// Increments the AccessFailedCount by 1 for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount++;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Resets the AccessFailedCount to 0 for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task ResetAccessFailedCountAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount = 0;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Sets LockoutEnabled for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEnabled = enabled;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Sets the LockoutEndDate for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEndDate = lockoutEnd;

            return Task.FromResult(0);
        }

        #endregion

        #region IUserLoginStore<TUser> Members

        /// <summary>
        /// Inserts an entry into the AspNetUserLogins table for the given TUser
        /// </summary>
        /// <param name="user">User to have claim added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            _userLoginsRepository.InsertLogin(user.Id, login);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns an TUser based on the UserLogininfo
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var userId = _userLoginsRepository.FindUserIdByLogin(login);

            if (userId == null) return Task.FromResult<TUser>(null);

            var user = _userRepository.GetUserById(userId);

            return user != null ? Task.FromResult((TUser) user) : Task.FromResult<TUser>(null);
        }

        /// <summary>
        /// Returns list of UserLoginInfo for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var userLogins = _userLoginsRepository.FindLoginsByUserId(user.Id);

            return Task.FromResult((IList<UserLoginInfo>) userLogins);
        }

        /// <summary>
        /// Deletes a login from AspNetUserLogins table for a given TUser
        /// </summary>
        /// <param name="user">User to have login removed</param>
        /// <param name="login">Login to be removed</param>
        /// <returns></returns>
        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            _userLoginsRepository.DeleteLogin(user.Id, login);

            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserPasswordStore<TUser> Members

        /// <summary>
        /// Returns the PasswordHash for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PasswordHash);
        }

        /// <summary>
        /// Verifies if user has password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> HasPasswordAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(!String.IsNullOrEmpty(user.PasswordHash));
        }

        /// <summary>
        /// Sets the password hash for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;

            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserPhoneNumberStore<TUser> Members

        /// <summary>
        /// Returns the PhoneNumber for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumber);
        }

        /// <summary>
        /// Returns PhoneNumberConfirmed for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        /// <summary>
        /// Set user's phone number
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (String.IsNullOrEmpty(phoneNumber))
                throw new ArgumentException("Null or empty argument: phoneNumber");

            user.PhoneNumber = phoneNumber;

            return Task.FromResult(0);
        }

        /// <summary>
        /// Set if user's phone number is confirmed
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumberConfirmed = confirmed;

            return Task.FromResult(0);
        }

        #endregion

        #region IUserRoleStore<TUser> Members

        /// <summary>
        /// Inserts an entry in the AspNetUserRoles table for the given user
        /// </summary>
        /// <param name="user">User to have role added</param>
        /// <param name="roleName">Name of the role to be added to user</param>
        /// <returns></returns>
        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            var roleId = _roleRepository.GetRoleId(roleName);

            if (String.IsNullOrEmpty(roleId))
                _userRolesRepository.InsertUserRole(user.Id, roleId);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns the roles for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var roles = _userRolesRepository.FindByUserId(user.Id);

            return Task.FromResult((IList<string>) roles);
        }

        /// <summary>
        /// Verifies if a user is in a role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            var roles = _userRolesRepository.FindByUserId(user.Id);

            return Task.FromResult(roles.Any(x => x == roleName));
        }

        /// <summary>
        /// Removes the given user from the given role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            var roleId = _roleRepository.GetRoleId(roleName);

            if (String.IsNullOrEmpty(roleId))
                _userRolesRepository.DeleteUserRole(user.Id, roleId);

            return Task.FromResult<object>(null);
        }

        #endregion

        #region IUserSecurityStampStore<TUser> Members

        /// <summary>
        /// Returns the SecurityStamp for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<string> GetSecurityStampAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        /// <summary>
        ///  Set security stamp  for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (String.IsNullOrEmpty(stamp))
                throw new ArgumentException("Null or empty argument: stamp");

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        #endregion

        #region IUserStore<TUser> Members
        /// <summary>
        /// Insert a new TUser in the AspNetUsers table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.InsertUser(user);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Delete a TUser from the AspNetUsers table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.DeleteUser(user.Id);

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns a TUser instance based on a userId query 
        /// </summary>
        /// <param name="userId">The user's Id</param>
        /// <returns></returns>
        public Task<TUser> FindByIdAsync(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentException("userId");

            var result = _userRepository.GetUserById(userId);

            return Task.FromResult((TUser) result);
        }

        /// <summary>
        /// Returns a TUser instance based on a userName query 
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public Task<TUser> FindByNameAsync(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("userName");

            var result = _userRepository.GetUserByName(userName).SingleOrDefault();

            return Task.FromResult((TUser) result);
        }

        /// <summary>
        /// Updates the AspNetUsers table with the TUser values
        /// </summary>
        /// <param name="user">TUser to be updated</param>
        /// <returns></returns>
        public Task UpdateAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.UpdateUser(user);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IUserTwoFactorStore<TUser,string> Members

        /// <summary>
        /// Returns TwoFactorEnabled for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.TwoFactorEnabled);
        }

        /// <summary>
        /// Set if two factor authentication is enabled for the user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.TwoFactorEnabled = enabled;

            return Task.FromResult(0);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}