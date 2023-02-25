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
    class ShowTradesCommand : ProtectedRouteCommand
    {
        private ITradeManager TradeManager { get; }

        public ShowTradesCommand(ITradeManager tradeManager)
        {
            TradeManager = tradeManager;
        }



        public override Response Execute()
        {
            IEnumerable<TradeLog> tradeLogs = TradeManager.ListTradeOffers();
            
            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Ok;
            response.Payload = JsonConvert.SerializeObject(tradeLogs);

            return response;
        }
    }
}

