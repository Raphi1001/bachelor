using Moq;
using NUnit.Framework;
using SWE1HttpServer.DAL.UserRepository;
using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.DAL.PackageRepository;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using SWE1HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.Managers.Packages;
using SWE1HttpServer.Models.Logs;
using Newtonsoft.Json;
using SWE1HttpServer.Managers.Cards;
using System.Reflection;
using SWE1HttpServer.Managers.Stacks;

namespace SWE1HttpServer.Test
{
    [TestFixture]
    class PackaManagerTest
    {
        [Test]
        public void AddValidPackageAsAdminCreatesPackage()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var packageRepo = new Mock<IPackageRepository>();
            var packageManager = new PackageManager(userRepo.Object, cardRepo.Object, packageRepo.Object);

            var user = new User("admin", "istrator");
            IList<CardLog> packageLog = new List<CardLog>();
            for (int i = 0; i < (int)Constants.packageSize; ++i)
            {
                packageLog.Add(new CardLog() { Id = "id" + i, Name = "FireDragon", Damage = 50 });
            }

            cardRepo.Setup(m => m.InsertPackageCards(It.IsAny<List<Card>>())).Returns(true);
            packageRepo.Setup(m => m.InsertPackage(It.IsAny<List<Card>>())).Returns(true);

            // act
            packageManager.AddPackage(user, packageLog);

            //assert
            cardRepo.Verify(m => m.InsertPackageCards(It.IsAny<List<Card>>()), Moq.Times.Once);
            packageRepo.Verify(m => m.InsertPackage(It.IsAny<List<Card>>()), Moq.Times.Once);
        }

        [Test]
        public void AddInvalidPackageThrowsBadRequestException()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var packageRepo = new Mock<IPackageRepository>();
            var packageManager = new PackageManager(userRepo.Object, cardRepo.Object, packageRepo.Object);

            var user = new User("admin", "istrator");
            IList<CardLog> packageLog = new List<CardLog>();
            for (int i = 0; i < (int)Constants.packageSize - 1; ++i)
            {
                packageLog.Add(new CardLog() { Id = "id" + i, Name = "FireDragon", Damage = 50 });
            }

            cardRepo.Setup(m => m.InsertPackageCards(It.IsAny<List<Card>>())).Returns(true);
            packageRepo.Setup(m => m.InsertPackage(It.IsAny<List<Card>>())).Returns(true);

            //act and assert
            Assert.Throws<BadRequestException>(() => packageManager.AddPackage(user, packageLog));

        }

        [Test]
        public void AddPackageAsInvalidAdminThrowsUnauthorizedAccessException()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var packageRepo = new Mock<IPackageRepository>();
            var packageManager = new PackageManager(userRepo.Object, cardRepo.Object, packageRepo.Object);

            var user = new User("testusr", "testpwd");
            IList<CardLog> packageLog = new List<CardLog>();
            for (int i = 0; i < (int)Constants.packageSize; ++i)
            {
                packageLog.Add(new CardLog() { Id = "id" + i, Name = "FireDragon", Damage = 50 });
            }
            // act and assert
            Assert.Throws<UnauthorizedAccessException>(() => packageManager.AddPackage(user, packageLog));
        }

        [Test]
        public void AquirePackageWithEnouthCoinsAquiresPackage()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var packageRepo = new Mock<IPackageRepository>();
            var packageManager = new PackageManager(userRepo.Object, cardRepo.Object, packageRepo.Object);

            var user = new User("admin", "istrator", (int)Constants.packagePrice, 67, 0, 0, 0);
            var cardId = "poqujhi345NB2w4th09wv";

            IList<string> samplePackage = new List<string>();
            for (int i = 0; i < (int)Constants.packageSize; ++i)
            {
                samplePackage.Add("id" + i);
            }

            userRepo.Setup(m => m.GetUserData(user.Token)).Returns(user);
            packageRepo.Setup(m => m.GetFirstPackage(user)).Returns(samplePackage);
            cardRepo.Setup(m => m.UpdateCardOwner(cardId, user.Username)).Returns(true);
            packageRepo.Setup(m => m.DeletePackage(cardId)).Returns(true);
            userRepo.Setup(m => m.UpdateUserCoins(user.Username, user.Coins)).Returns(true);

            // act
            packageManager.AcquirePackage(user);

            //assert
            cardRepo.Verify(m => m.UpdateCardOwner(It.IsIn<string>(samplePackage), user.Username), Moq.Times.Exactly((int)Constants.packageSize));
            packageRepo.Verify(m => m.DeletePackage(It.IsIn<string>(samplePackage)), Moq.Times.Once);
            userRepo.Verify(m => m.UpdateUserCoins(user.Username, user.Coins), Moq.Times.Once);

        }

        [Test]
        public void AquirePackageWithNotEnouthCoinsThrowsConflictException()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var packageRepo = new Mock<IPackageRepository>();
            var packageManager = new PackageManager(userRepo.Object, cardRepo.Object, packageRepo.Object);

            var user = new User("admin", "istrator", ((int)Constants.packagePrice - 1), 67, 0, 0, 0);
           
            userRepo.Setup(m => m.GetUserData(user.Token)).Returns(user);

            // act and assert
            Assert.Throws<ConflictException>(() => packageManager.AcquirePackage(user));
        }

        [Test]
        public void AquirePackageWithNoPackagesLeftThrowsConflictException()
        {
            //arrange
            var userRepo = new Mock<IUserRepository>();
            var cardRepo = new Mock<ICardRepository>();
            var packageRepo = new Mock<IPackageRepository>();
            var packageManager = new PackageManager(userRepo.Object, cardRepo.Object, packageRepo.Object);

            var user = new User("admin", "istrator", ((int)Constants.packagePrice - 1), 67, 0, 0, 0);
            
            IList<string> expected = null;
            userRepo.Setup(m => m.GetUserData(user.Token)).Returns(user);
            packageRepo.Setup(m => m.GetFirstPackage(user)).Returns(expected);

            // act and assert
            Assert.Throws<ConflictException>(() => packageManager.AcquirePackage(user));
        }
    }
}
