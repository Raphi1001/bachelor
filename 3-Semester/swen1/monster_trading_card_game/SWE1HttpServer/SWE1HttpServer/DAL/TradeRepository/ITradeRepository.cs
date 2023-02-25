using SWE1HttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.DAL.TradeRepository
{
    public interface ITradeRepository
    {
        Trade GetTradeByCardId(string cardId);
        Trade GetTradeById(string tradeId);
        IEnumerable<Trade> GetAllTradeOffers();
        bool InsertTrade(Trade trade);
        bool DeleteTrade(string tradeId);
    }
}
