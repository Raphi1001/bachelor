using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL.CardRepository
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards(string username);

        IList<Card> GetAllDeckCards(string username, IList<string> cardIds);

        Card GetCardById(string cardId);
        string GetCardOwnerById(string cardId);
        bool InsertPackageCards(IList<Card> cards);
        bool UpdateCardOwner(string cardId, string username);
        bool SwapCardOwner(string card1Id, string card2Id, string card1Owner, string card2Owner);


        //IEnumerable<CardCredentials> GetAllDeckCards(string username);
        //bool UpdateCardDeckStatus(string cardId, string username, bool status);
        // void UpdateDeck(IList<string> cardIds, string username);


    }
}
