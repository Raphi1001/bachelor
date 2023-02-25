using SWE1HttpServer.Managers;
using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models
{
    public class Trade
    {
        public string Id { get; private set; }
        public string CardId { get; private set; }
        public CardType RequiredCardType { get; private set; }
        public int RequiredMinimumDamage { get; private set; }


        public Trade(string id, string cardIdToTrade, CardType requiredCardType, int requiredMinimumDamage)
        {
            StringValidator.Validate(id);
            StringValidator.Validate(cardIdToTrade);
            if (requiredMinimumDamage < 0)
                throw new InvalidException();

            Id = id;
            CardId = cardIdToTrade;
            RequiredCardType = requiredCardType;
            RequiredMinimumDamage = requiredMinimumDamage;
        }
    }
}
