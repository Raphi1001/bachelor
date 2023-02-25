using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Trades
{
    public interface ITradeManager
    {
        IEnumerable<TradeLog> ListTradeOffers();
        void CreateTradeOffer(User user, TradeLog trade);
        void AcceptTradeOffer(User user, string tradeId, string cardId);
        void DeleteTradeOffer(string tradeId);
    }
}
