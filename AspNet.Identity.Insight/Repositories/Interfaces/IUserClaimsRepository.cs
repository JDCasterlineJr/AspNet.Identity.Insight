using System.Security.Claims;

namespace AspNet.Identity.Insight.Repositories.Interfaces
{
    public interface IUserClaimsRepository
    {
        void InsertClaim(string userId, Claim userClaim);
        void DeleteClaim(string userId, Claim claim);
        ClaimsIdentity FindClaimsByUserId(string userId);
    }
}