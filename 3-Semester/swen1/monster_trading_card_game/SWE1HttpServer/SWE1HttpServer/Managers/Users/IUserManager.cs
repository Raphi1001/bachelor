using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System.Collections.Generic;

namespace SWE1HttpServer.Managers.Users
{
    public interface IUserManager
    {
        User LoginUser(UserCredentialsLog credentials);
        void RegisterUser(UserCredentialsLog credentials);
        void EditUserData(string Username, User user, UserDataLog userDataCredentials);
        UserDataLog ShowUserData(string username, User user);
        StatsLog ShowStats(User user);
        IEnumerable<int> ShowScoreboard();
    }
}