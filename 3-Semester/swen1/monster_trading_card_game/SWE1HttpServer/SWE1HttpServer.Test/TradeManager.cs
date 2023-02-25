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
using SWE1HttpServer.DAL.TradeRepository;
using SWE1HttpServer.Managers.Trades;
using SWE1HttpServer.DAL.DeckRepository;

namespace SWE1HttpServer.Test
{
    [TestFixture]
    class TradeManagerTest
    {
        [Test]
        public void ListTradeOffersWithExistingTradesRetunsTradeLog()
        {
            //arrange
            var cardRepo = new Mock<ICardRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var tradeManager = new TradeManager(cardRepo.Object, deckRepo.Object, tradeRepo.Object);

            IEnumerable<Trade> trades = new List<Trade>();
            Trade trade = new Trade("id", "ahdahd", CardType.monster, 15);
            trades = trades.Append(trade);
            tradeRepo.Setup(m => m.GetAllTradeOffers()).Returns(trades);

            // act
            IEnumerable<TradeLog> actuall = tradeManager.ListTradeOffers();

            //assert
            Assert.IsTrue(
                     actuall.First().Id == trade.Id &&
                     actuall.First().Type == trade.RequiredCardType.ToString() &&
                     actuall.First().MinimumDamage == trade.RequiredMinimumDamage);
        }

        [Test]
        public void ListTradeOffersWithNoExistingTradesThrowsNoContentException()
        {
            //arrange
            var cardRepo = new Mock<ICardRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var tradeManager = new TradeManager(cardRepo.Object, deckRepo.Object, tradeRepo.Object);

            IEnumerable<Trade> trades = new List<Trade>();
            tradeRepo.Setup(m => m.GetAllTradeOffers()).Returns(trades);

            // act and assert
            Assert.Throws<NoContentException>(() => tradeManager.ListTradeOffers());
        }

        [Test]
        public void CreateValidTradeOffer()
        {
            //arrange
            var cardRepo = new Mock<ICardRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var tradeManager = new TradeManager(cardRepo.Object, deckRepo.Object, tradeRepo.Object);

            User user = new User("testis", "123sicher");
            TradeLog trade = new TradeLog() { Id = "12ajsdhakjda", CardToTrade = "idvonderkarte", Type = "monster", MinimumDamage = 5 };
            IList<string> userDeck = new List<string>();
            for (int i = 0; i < (int)Constants.deckSize; ++i)
            {
                userDeck.Add("id" + i);
            }

            cardRepo.Setup(m => m.GetCardOwnerById(trade.CardToTrade)).Returns(user.Username);
            deckRepo.Setup(m => m.GetUserDeckIds(user.Username)).Returns(userDeck);
            tradeRepo.Setup(m => m.InsertTrade(It.Is<Trade>(t => 
            
            t.Id == trade.Id &&
            t.CardId == trade.CardToTrade &&
            t.RequiredCardType.ToString() == trade.Type && 
            t.RequiredMinimumDamage == trade.MinimumDamage))).Returns(true);
            
            // act 
            tradeManager.CreateTradeOffer(user, trade);

            //assert
            cardRepo.Verify(m => m.GetCardOwnerById(trade.CardToTrade), Moq.Times.Once);
            deckRepo.Verify(m => m.GetUserDeckIds(user.Username), Moq.Times.Once);
            tradeRepo.Verify(m => m.InsertTrade(It.Is<Trade>(t =>
                t.Id == trade.Id &&
                t.CardId == trade.CardToTrade &&
                t.RequiredCardType.ToString() == trade.Type &&
                t.RequiredMinimumDamage == trade.MinimumDamage)), Moq.Times.Once);

        }

        [Test]
        public void CreateTradeOfferWithDeckCardThrowsForbiddenException()
        {
            //arrange
            var cardRepo = new Mock<ICardRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var tradeManager = new TradeManager(cardRepo.Object, deckRepo.Object, tradeRepo.Object);

            User user = new User("testis", "123sicher");
            TradeLog trade = new TradeLog() { Id = "tradeid", CardToTrade = "id0", Type = "monster", MinimumDamage = 5 };
            IList<string> userDeck = new List<string>();
            for (int i = 0; i < (int)Constants.deckSize; ++i)
            {
                userDeck.Add("id" + i);
            }

            cardRepo.Setup(m => m.GetCardOwnerById(trade.CardToTrade)).Returns(user.Username);
            deckRepo.Setup(m => m.GetUserDeckIds(user.Username)).Returns(userDeck);
          
            // act and assert
            Assert.Throws<ForbiddenException>(() => tradeManager.CreateTradeOffer(user, trade));

        }

        [Test]
        public void CreateTradeOfferWithNotOwnedCardThrowsForbiddenException()
        {
            //arrange
            var cardRepo = new Mock<ICardRepository>();
            var deckRepo = new Mock<IDeckRepository>();
            var tradeRepo = new Mock<ITradeRepository>();
            var tradeManager = new TradeManager(cardRepo.Object, deckRepo.Object, tradeRepo.Object);

            User user = new User("testis", "123sicher");
            TradeLog trade = new TradeLog() { Id = "tradeid", CardToTrade = "id0", Type = "monster", MinimumDamage = 5 };
            IList<string> userDeck = new List<string>();
            for (int i = 0; i < (int)Constants.deckSize; ++i)
            {
                userDeck.Add("id" + i);
            }

            cardRepo.Setup(m => m.GetCardOwnerById(trade.CardToTrade)).Returns("different user");

            // act and assert
            Assert.Throws<ForbiddenException>(() => tradeManager.CreateTradeOffer(user, trade));

        }
    }
}
