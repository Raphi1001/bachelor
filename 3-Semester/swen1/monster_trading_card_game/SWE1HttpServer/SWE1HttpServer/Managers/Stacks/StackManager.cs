using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Managers.Cards;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Stacks
{
    public class StackManager : IStackManager
    {
        private ICardRepository CardRepository { get; }

        public StackManager(ICardRepository cardRepository)
        {
            CardRepository = cardRepository;
        }
        public IEnumerable<CardLog> ListStackLog(User user)
        {
            IEnumerable<Card> stackCards = CardRepository.GetAllCards(user.Username);
            if (!stackCards.Any())
                throw new NoContentException();
           
            IEnumerable<CardLog> stackCardCredentials = new List<CardLog>();
            foreach (Card card in stackCards)
            {
                CardLog cardCredentials = CardManager.ConvertCardToCardCredentials(card);
                stackCardCredentials = stackCardCredentials.Append(cardCredentials);
            }

            return stackCardCredentials;
        }
    }
}
