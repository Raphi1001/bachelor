using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.DAL.DeckRepository;
using SWE1HttpServer.DAL.TradeRepository;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Trades
{
    public class TradeManager : ITradeManager
    {
        private ICardRepository CardRepository { get; }
        private IDeckRepository DeckRepository { get; }
        private ITradeRepository TradeRepository { get; }
        public TradeManager(ICardRepository cardRepository, IDeckRepository deckRepository, ITradeRepository tradeRepository)
        {
            CardRepository = cardRepository;
            DeckRepository = deckRepository;
            TradeRepository = tradeRepository;
        }
        public static TradeLog ConvertTradeToTradeLog(Trade trade)
        {
            TradeLog tradelog = new TradeLog
            {
                Id = trade.Id,
                CardToTrade = trade.CardId,
                Type = trade.RequiredCardType.ToString(),
                MinimumDamage = trade.RequiredMinimumDamage
            };

            return tradelog;
        }

        public static Trade ConvertTradeLogToTrade(TradeLog tradeLog)
        {
            return CreateTrade(tradeLog.Id, tradeLog.CardToTrade, tradeLog.Type, tradeLog.MinimumDamage);
        }

        public static Trade CreateTrade(string id, string cardIdToTrade, string requiredCardType, int requiredMinimumDamage)
        {
            Trade trade = null;

            if (Enum.TryParse(requiredCardType?.ToLower(), out CardType cardType))
            {
                trade = new Trade(id, cardIdToTrade, cardType, requiredMinimumDamage);
            }

            if (trade is null)
                throw new InvalidException();

            return trade;
        }

        public IEnumerable<TradeLog> ListTradeOffers()
        {
            IEnumerable<Trade> trades = TradeRepository.GetAllTradeOffers();
            if (!trades.Any())
            {
                throw new NoContentException();
            }

            IEnumerable<TradeLog> tradeLogs = new List<TradeLog>();
            foreach (Trade trade in trades)
            {
                TradeLog tradeLog = TradeManager.ConvertTradeToTradeLog(trade);
                tradeLogs = tradeLogs.Append(tradeLog);
            }

            return tradeLogs;
        }

        public void CreateTradeOffer(User user, TradeLog tradeLog)
        {
            Trade trade = TradeManager.ConvertTradeLogToTrade(tradeLog);

            //check if user owns card
            if (user.Username != CardRepository.GetCardOwnerById(trade.CardId))
            {
                throw new ForbiddenException();
            }

            //check if user has card in deck
            IList<string> deck = DeckRepository.GetUserDeckIds(user.Username);
            if (deck is not null && deck.Contains(trade.CardId))
            {
                throw new ForbiddenException();
            }
            Console.WriteLine("ja");
            TradeRepository.InsertTrade(trade);
        }

        public void AcceptTradeOffer(User user, string tradeId, string cardId)
        {
            StringValidator.Validate(tradeId);
            StringValidator.Validate(cardId);

            Trade trade = TradeRepository.GetTradeById(tradeId);

            //check if trade exists
            if (trade is null)
            {
                throw new BadRequestException();
            }
            string tradeOwner = CardRepository.GetCardOwnerById(trade.CardId);

            //check if user owns trade
            if (user.Username == tradeOwner)
            {
                throw new ForbiddenException();
            }

            //check if user owns card
            if (user.Username != CardRepository.GetCardOwnerById(cardId))
            {
                Console.WriteLine(user.Username);
                Console.WriteLine(CardRepository.GetCardOwnerById(cardId));
                throw new ForbiddenException();
            }

            //check if user has card in deck
            IList<string> deck = DeckRepository.GetUserDeckIds(user.Username);
            if (deck is not null && deck.Contains(cardId))
            {
                throw new ForbiddenException();
            }

            Card card = CardRepository.GetCardById(cardId);
            //chek if card meets specified trade requirements
            if (card.CardType != trade.RequiredCardType || card.Damage < trade.RequiredMinimumDamage)
            {
                throw new ForbiddenException();
            }

            if (CardRepository.SwapCardOwner(trade.CardId, cardId, tradeOwner, user.Username))

                if (!TradeRepository.DeleteTrade(trade.Id))
                {
                    throw new InternalServerErrorException();
                }
        }

        public void DeleteTradeOffer(string tradeId)
        {
            StringValidator.Validate(tradeId);

            if (!TradeRepository.DeleteTrade(tradeId))
                throw new ConflictException();
        }


    }
}
