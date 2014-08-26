using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using AspNet.Identity.Insight.Repositories.Interfaces;
using Insight.Database;

namespace AspNet.Identity.Insight.Repositories
{
    public class UserClaimsRepository : IUserClaimsRepository
    {
        private readonly IDbConnection _database;

        public UserClaimsRepository(IDbConnection database)
        {
            _database = database;
        }

        public UserClaimsRepository()
            : this(new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString).Connection())
        {
        }

        public void InsertClaim(string userId, Claim userClaim)
        {
            _database.ExecuteSql(
                "INSERT INTO AspNetUserClaims (UserId, ClaimType, ClaimValue) VALUES (@userId, @type, @value)",
                new { userId, type = userClaim.Type, value = userClaim.Value });
        }

        public void DeleteClaim(string userId, Claim claim)
        {
            _database.ExecuteSql(
                "DELETE FROM AspNetUserClaims WHERE UserId = @userId AND ClaimType = @type AND ClaimValue = @value",
                new { userId, type = claim.Type, value = claim.Value });
        }

        public ClaimsIdentity FindClaimsByUserId(string userId)
        {
            var result = _database.QuerySql(
                "SELECT * FROM AspNetUserClaims WHERE UserId = @userId",
                new { userId });

            var claims = new ClaimsIdentity();

            claims.AddClaims(
                result.Select(x => new Claim(x["ClaimType"].ToString(), x["ClaimValue"].ToString()))
                    .ToList());

            return claims;
        }
    }
}
