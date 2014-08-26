using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Identity.Insight.Repositories;
using Insight.Database;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Insight
{
    public class RoleStore<TRole> : 
        IRoleStore<TRole>,
        IQueryableRoleStore<TRole> 
        where TRole: IdentityRole
    {
        private readonly RoleRepository _roleRepository;

        public RoleStore()
            : this(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Connection())
        {
        }

        public RoleStore(IDbConnection database)
        {
             _roleRepository = new RoleRepository(database);
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.InsertRole(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.DeleteRole(role.Id);

            return Task.FromResult<object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
                throw new ArgumentException("Null or empty argument: roleId");

            var result = _roleRepository.GetRoleById(roleId);

            return Task.FromResult((TRole)result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentException("Null or empty argument: roleName");

            var result = _roleRepository.GetRoleByName(roleName);

            return Task.FromResult((TRole)result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.UpdateRole(role);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        public IQueryable<TRole> Roles
        {
            get { return (IQueryable<TRole>)_roleRepository.GetAllRoles().AsQueryable(); }
        }
    }
}
