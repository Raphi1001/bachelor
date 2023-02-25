using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Decks
{
    public interface IDeckManager
    {
        IList<CardLog> ListDeck(User user);
        void ConfigureDeck(IList<string> cardIds, User user);

    }
}
