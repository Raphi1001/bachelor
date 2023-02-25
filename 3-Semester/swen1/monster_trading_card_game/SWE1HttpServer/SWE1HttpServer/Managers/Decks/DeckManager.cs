using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.DAL.DeckRepository;
using SWE1HttpServer.DAL.TradeRepository;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Managers.Cards;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Decks
{
    public class DeckManager : IDeckManager
    {
        private ICardRepository CardRepository { get; }
        private IDeckRepository DeckRepository { get; }
        private ITradeRepository TradeRepository { get; }



        public DeckManager(ICardRepository cardRepository, IDeckRepository deckRepository, ITradeRepository tradeRepository)
        {
            CardRepository = cardRepository;
            DeckRepository = deckRepository;
            TradeRepository = tradeRepository;
        }
        public IList<CardLog> ListDeck(User user)
        {
            IList<string> cardIds = DeckRepository.GetUserDeckIds(user.Username);

            if (cardIds is null)
            {
                throw new NoContentException();
            }

            IList<CardLog> deck = new List<CardLog>();

            foreach (string cardId in cardIds)
            {
                Card card = CardRepository.GetCardById(cardId);
                deck.Add(CardManager.ConvertCardToCardCredentials(card));
            }

            if (cardIds.Count != (int)Constants.deckSize)
            {
                throw new InternalServerErrorException();
            }

            return deck;
        }

        public void ConfigureDeck(IList<string> cardIds, User user)
        {
            if (cardIds?.Count != (int)Constants.deckSize)
            {
                throw new BadRequestException();
            }

            foreach (string cardId in cardIds)
            {
                StringValidator.Validate(cardId);
                //check if user owns all specified cards
                if (user.Username != CardRepository.GetCardOwnerById(cardId))
                    throw new ForbiddenException();

                //check if cards are locked by trade
                if (TradeRepository.GetTradeByCardId(cardId) is not null)
                    throw new ForbiddenException();
            }

            DeckRepository.DeleteDeck(user.Username);
            DeckRepository.InsertDeck(cardIds, user.Username);
        }

    }
}
