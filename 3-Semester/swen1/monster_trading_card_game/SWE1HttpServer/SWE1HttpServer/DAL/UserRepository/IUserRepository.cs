using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL.UserRepository
{
    public interface IUserRepository
    {
        User GetUserByCredentials(string username, string password);

        User GetUserByAuthToken(string authToken);

        User GetUserData(string authToken);
        bool InsertUser(User user);
        bool UpdateUserData(string token, UserDataLog userDataCredentials);
        bool UpdateUserCoins(string username, int coins);
        bool UpdateUserScore(string username, int elo, int playedGames, int wonGames, int lostGames);


        IEnumerable<int> GetAllUserStats();


    }
}
