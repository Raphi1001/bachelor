using SWE1HttpServer.DAL.UserRepository;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Users
{
    public class UserManager : IUserManager
    {
        private IUserRepository UserRepository { get; }

        public UserManager(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public static User ConvertUserCredentialsLogToUser(UserCredentialsLog userCredentialsLog)
        {
            User user = new User(userCredentialsLog.Username, userCredentialsLog.Password);

            return user;
        }

        public static UserDataLog ConvertUserToUserDataLog(User user)
        {
            UserDataLog userDataCredentials = new UserDataLog
            {
                Name = user.Name,
                Bio = user.Bio,
                Image = user.Image
            };

            return userDataCredentials;
        }

        public User LoginUser(UserCredentialsLog credentials)
        {
            credentials.Validate();
            User user = UserRepository.GetUserByCredentials(credentials.Username, credentials.Password);
            if (user is null)
                throw new UnauthorizedAccessException();

            return user;
        }

        public void RegisterUser(UserCredentialsLog credentials)
        {
            credentials.Validate();
            User user = new User(credentials.Username, credentials.Password);

            if (UserRepository.InsertUser(user) == false)
            {
                throw new ForbiddenException();
            }
        }
        public void EditUserData(string username, User user, UserDataLog userData)
        {
            StringValidator.Validate(username);
            userData.Validate();

            if (user.Username != username)
                throw new ForbiddenException();

            user.SetUserData(userData.Name, userData.Bio, userData.Image);

            try
            {
                UserRepository.UpdateUserData(user.Token, userData);
            }
            catch (InvalidException)
            {
                throw new InternalServerErrorException();
            }
        }

        public UserDataLog ShowUserData(string username, User user)
        {
            StringValidator.Validate(username);

            if (user.Username != username)
            {
                throw new ForbiddenException();
            }


            user = UserRepository.GetUserData(user.Token);
            if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Image) && string.IsNullOrEmpty(user.Bio))
                throw new NoContentException();


            if (user is null)
                throw new InternalServerErrorException();

            return ConvertUserToUserDataLog(user);

        }

        public StatsLog ShowStats(User user)
        {
            user = UserRepository.GetUserData(user.Token);
            StatsLog stats = new StatsLog() { EloScore = user.EloScore, PlayedGames = user.PlayedGames, WonGames = user.WonGames, LostGames = user.LostGames };
            //check for division by 0
            if (user.LostGames != 0)
                stats.WinLooseRatio = (user.WonGames / user.LostGames);
            else
                stats.WinLooseRatio = user.WonGames;

            return stats;
        }

        public IEnumerable<int> ShowScoreboard()
        {
            IEnumerable<int> scoreboard = UserRepository.GetAllUserStats();
            return scoreboard;
        }
    }
}
