using Moq;
using NUnit.Framework;
using SWE1HttpServer.DAL.UserRepository;
using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.DAL.PackageRepository;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.Managers.Packages;
using Newtonsoft.Json;
using SWE1HttpServer.Managers.Cards;
using System.Reflection;
using SWE1HttpServer.Managers.Stacks;
using SWE1HttpServer.Managers.Users;

namespace SWE1HttpServer.Test
{
    [TestFixture]
    class UserManagerTest
    {
        [Test]
        public void LoginUserWithValidUserReturnsUser()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);

            var userCredentials = new UserCredentialsLog() { Username = "harry", Password = "savePassword" };
            var expected = UserManager.ConvertUserCredentialsLogToUser(userCredentials);

            userRepo.Setup(m => m.GetUserByCredentials(userCredentials.Username, userCredentials.Password)).Returns(expected);

            // act
            var actuall = userManager.LoginUser(userCredentials);

            //assert
            Assert.AreEqual(actuall, expected);
        }

        [Test]
        public void LoginUserWithInValidUserThrowsUnauthorizedAccessException()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);

            var userCredentials = new UserCredentialsLog() { Username = "harry", Password = "savePassword" };
            User expected = null;

            userRepo.Setup(m => m.GetUserByCredentials(userCredentials.Username, userCredentials.Password)).Returns(expected);


            //assert and act
            Assert.Throws<UnauthorizedAccessException>(() => userManager.LoginUser(userCredentials));

        }
        [Test]
        public void RegisterUserWithValidUserCreatesUser()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);

            var userCredentials = new UserCredentialsLog() { Username = "harry", Password = "savePassword" };
            userRepo.Setup(m => m.InsertUser(It.Is<User>(u => u.Username == userCredentials.Username && u.Password == userCredentials.Password))).Returns(true);

            //act
            userManager.RegisterUser(userCredentials);

            //assert 
            userRepo.Verify(m => m.InsertUser(It.Is<User>(u => u.Username == userCredentials.Username && u.Password == userCredentials.Password)), Moq.Times.Once);
        }

        [Test]
        public void RegisterUserWithDuplicateUserThrowsForbiddenException()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);

            var userCredentials = new UserCredentialsLog() { Username = "harry", Password = "savePassword" };
            userRepo.Setup(m => m.InsertUser(It.Is<User>(u => u.Username == userCredentials.Username && u.Password == userCredentials.Password))).Returns(false);

            //act and assert
            Assert.Throws<ForbiddenException>(() => userManager.RegisterUser(userCredentials));

        }

        [Test]
        public void EditUserDataWithValidUsernameEditsUser()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var userName = "maria123";
            var user = new User(userName, "savePassword");
            var userData = new UserDataLog() { Name = "Maria", Bio = "I ma", Image = ":)" };
            userRepo.Setup(m => m.UpdateUserData(user.Token, userData)).Returns(true);

            //act
            userManager.EditUserData(userName, user, userData);

            //assert 
            userRepo.Verify(m => m.UpdateUserData(user.Token, userData), Moq.Times.Once);
        }

        [Test]
        public void EditUserDataWithInvalidUsernameThrowsForbiddenException()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var userName = "maria123";
            var user = new User("different", "savePassword");
            var userData = new UserDataLog() { Name = "Maria", Bio = "I ma", Image = ":)" };
            userRepo.Setup(m => m.UpdateUserData(user.Token, userData)).Returns(true);

            //act
            Assert.Throws<ForbiddenException>(() => userManager.EditUserData(userName, user, userData));
        }

      
        [Test]
        public void ShowUserDataWithNoUserDataThrowsNoContentException()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var userName = "maria123";
            var user = new User(userName, "savePassword");

            userRepo.Setup(m => m.GetUserData(user.Token)).Throws(new InvalidException());

            //act and assert
            Assert.Throws<InvalidException>(() => userManager.ShowUserData(userName, user));
        }

        [Test]
        public void ShowUserDataWithInvalidUsernameThrowsForbiddenException()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);
            var userName = "maria123";
            var user = new User("different", "savePassword");

            userRepo.Setup(m => m.GetUserData(user.Token)).Returns(user);

            //act and assert
            Assert.Throws<ForbiddenException>(() => userManager.ShowUserData(userName, user));
        }


        [Test]
        public void ShowStatsReturnsStats()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);

            var user = new User("testusr", "testpwd", 5, 150, 0, 0, 1);
   

            userRepo.Setup(m => m.GetUserData(user.Token)).Returns(user);

            // act
            var actuall = userManager.ShowStats(user);

            //assert
            Assert.AreEqual(actuall.EloScore, user.EloScore);
            Assert.AreEqual(actuall.PlayedGames, user.PlayedGames);
            Assert.AreEqual(actuall.WonGames, user.WonGames);
            Assert.AreEqual(actuall.LostGames, user.LostGames);
            Assert.AreEqual(actuall.WinLooseRatio, user.WonGames / (user.LostGames != 0 ? user.LostGames : 0));

        }

        [Test]
        public void ShowScoreboardReturnsScoreboard()
        {
            // arrange
            var userRepo = new Mock<IUserRepository>();
            var userManager = new UserManager(userRepo.Object);

            IList<int> expected = new List<int>();
            expected.Add(280);

            userRepo.Setup(m => m.GetAllUserStats()).Returns(expected);

            // act
            var actuall = userManager.ShowScoreboard();

            //assert
            Assert.AreEqual(actuall, expected);
        }
    }
}
