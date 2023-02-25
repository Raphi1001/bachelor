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
using SWE1HttpServer.Managers.Trades;

namespace SWE1HttpServer.RouteCommands.Trades
{
    class AcceptTradeCommand : ProtectedRouteCommand
    {
        private ITradeManager TradeManager { get; }
        public string TradeId { get; }
        public string CardId { get; }
        public AcceptTradeCommand(ITradeManager tradeManager, string tradeId, string cardId)
        {
            TradeManager = tradeManager;
            TradeId = tradeId;
            CardId = cardId;
        }



        public override Response Execute()
        {
            TradeManager.AcceptTradeOffer(User, TradeId, CardId);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;

            return response;
        }
    }
}

