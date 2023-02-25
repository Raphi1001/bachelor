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
    class StackManagerTest
    {
        [Test]
        public void ListStackWithValidStackReturnsStackLog()
        {
            // arrange
            var cardRepo = new Mock<ICardRepository>();
            var stackManager = new StackManager(cardRepo.Object);

            var user = new User("testusr", "testpwd");

            IList<Card> sampleStack = new List<Card>();
            sampleStack.Add(new MonsterCard("superUniqueId", MonsterName.dragon, 69, ElementType.fire));

            CardLog expectedCardLog = new CardLog() { Id = "superUniqueId", Name = "FireDragon", Damage = 69 };

            cardRepo.Setup(m => m.GetAllCards(user.Username)).Returns(sampleStack);

            // act
            var actuall = stackManager.ListStackLog(user);

            //assert
            Assert.IsTrue(
                string.Equals(actuall.First().Id, expectedCardLog.Id) &&
                string.Equals(actuall.First().Name, expectedCardLog.Name) &&
                Equals(actuall.First().Damage, expectedCardLog.Damage)
                );
        }

        [Test]
        public void ListStackWithEmptyStackThrowsNoContentException()
        {
            // arrange
            var cardRepo = new Mock<ICardRepository>();
            var stackManager = new StackManager(cardRepo.Object);

            var user = new User("testusr", "testpwd");

            IList<Card> sampleStack = new List<Card>();

            CardLog expectedCardLog = new CardLog() { Id = "superUniqueId1", Name = "FireDragon", Damage = 69 };

            // act and assert
            Assert.Throws<NoContentException>(() => stackManager.ListStackLog(user));
        }
    }
}
