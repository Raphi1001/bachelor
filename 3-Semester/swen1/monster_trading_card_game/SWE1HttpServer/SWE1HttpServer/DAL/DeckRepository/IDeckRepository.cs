using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL.DeckRepository
{
    public interface IDeckRepository
    {
        IList<string> GetUserDeckIds(string username);

        bool DeleteDeck(string username);
        bool InsertDeck(IList<string> cardIds, string username);

        void UpdateDeck(IList<string> deckCards, string username);
    }
}
