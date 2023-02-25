using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SWE1HttpServer.Managers.Decks;

namespace SWE1HttpServer.RouteCommands.Deck
{
    class ConfigureDeckCommand : ProtectedRouteCommand
    {
        private IDeckManager DeckManager { get; }
        public IList<string> DeckCards { get; }
        public ConfigureDeckCommand(IDeckManager deckManager, IList<string> deckCards)
        {
            DeckManager = deckManager;
            DeckCards = deckCards;
        }

        public override Response Execute()
        {
            DeckManager.ConfigureDeck(DeckCards, User);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;

            return response;
        }
    }
}
