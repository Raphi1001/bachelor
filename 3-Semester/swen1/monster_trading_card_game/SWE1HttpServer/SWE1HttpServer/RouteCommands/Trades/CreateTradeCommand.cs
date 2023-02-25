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
    class CreateTradeCommand : ProtectedRouteCommand
    {
        private ITradeManager TradeManager { get; }

        public TradeLog TradeLog { get; }
        public CreateTradeCommand(ITradeManager tradeManager, TradeLog tradeLog)
        {
            TradeManager = tradeManager;
            TradeLog = tradeLog;
        }



        public override Response Execute()
        {
            TradeManager.CreateTradeOffer(User, TradeLog);

            //this is only reached if no exception is thrown
            var response = new Response();
            response.StatusCode = StatusCode.Created;
            return response;
        }
    }
}

