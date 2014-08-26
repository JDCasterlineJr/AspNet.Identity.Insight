using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Insight.Repositories.Interfaces
{
    public interface IUserLoginsRepository
    {
        void InsertLogin(string userId, UserLoginInfo login);
        void DeleteLogin(string userId, UserLoginInfo login);
        string FindUserIdByLogin(UserLoginInfo userLogin);
        IEnumerable<UserLoginInfo> FindLoginsByUserId(string userId);
    }
}