using SWE1HttpServer.Core.Response;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SWE1HttpServer.Models;
using SWE1HttpServer.Managers.Decks;

namespace SWE1HttpServer.RouteCommands.Deck
{
    class ShowDeckCommand : ProtectedRouteCommand
    {
        private IDeckManager DeckManager { get; }
        private readonly ResponseDeckFormatType responseFormat;

        public ShowDeckCommand(IDeckManager deckManager)
        {
            DeckManager = deckManager;
            responseFormat = ResponseDeckFormatType.json;
        }

        public ShowDeckCommand(IDeckManager deckManager, string format)
        {
            DeckManager = deckManager;

            if (Enum.TryParse(format.ToLower(), out ResponseDeckFormatType responseFormat))
            {
                this.responseFormat = responseFormat;
                return;
            }
            this.responseFormat = ResponseDeckFormatType.json;
        }

        public override Response Execute()
        {

            IList<CardLog> deckLog = DeckManager.ListDeck(User);
            
            //this is only reached if no exception is thrown
            var response = new Response();
            switch (responseFormat)
            {
                case ResponseDeckFormatType.plain:
                    response.Payload = ShowDeckFormatPlain(deckLog);
                    break;
                default:
                    response.Payload = ShowDeckFormatJson(deckLog);
                    break;
            }

            response.StatusCode = StatusCode.Ok;

            return response;
        }


        private string ShowDeckFormatPlain(IList<CardLog> deckLog)
        {
            var payload = new StringBuilder();

            foreach (CardLog cardLog in deckLog)
            {
                payload.Append("Id: ");
                payload.Append(cardLog.Id);
                payload.Append(", CardName: ");
                payload.Append(cardLog.Name);
                payload.Append(", Damage: ");
                payload.Append(cardLog.Damage);
                payload.Append('\n');
            }

            return payload.ToString();
        }

        private string ShowDeckFormatJson(IList<CardLog> deck)
        {
            return JsonConvert.SerializeObject(deck);
        }
    }
}

