using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Managers.Cards
{
    public class CardManager : ICardManager
    {
        public static Card CreateCard(string id, string elementName, string cardName, double damage)
        {
            Card card = null;

            if (Enum.TryParse(elementName?.ToLower(), out ElementType elementType))
            {
                if (Enum.TryParse(cardName?.ToLower(), out MonsterName monsterName))
                {
                    card = new MonsterCard(id, monsterName, damage, elementType);

                }
                else if (Enum.TryParse(cardName?.ToLower(), out SpellName spellName))
                {
                    card = new SpellCard(id, spellName, damage, elementType);

                }
            }

            if (card is null)
                throw new InvalidException();

            return card;
        }

        public static Card ConvertCardCredentialsToCard(CardLog cardCredentials)
        {
            cardCredentials.Validate();
            string elementName = ElementType.regular.ToString();
            string cardName = cardCredentials.Name;

            //split element and card name
            for (int i = 1; i < cardCredentials.Name.Length; ++i)  //skips first char
            {
                if (char.IsUpper(cardCredentials.Name[i]))
                {
                    elementName = cardCredentials.Name.Substring(0, i);
                    cardName = cardCredentials.Name.Substring(i);
                    break;
                }
            }
            return CreateCard(cardCredentials.Id, elementName, cardName, cardCredentials.Damage);
        }

        public static CardLog ConvertCardToCardCredentials(Card card)
        {
            string cardName = FirstLetterToUpper(card.Name);
            string elementName = FirstLetterToUpper(card.ElementType.ToString());

            CardLog cardCredentials = new CardLog
            {
                Id = card.Id,
                Name = elementName + cardName,
                Damage = card.Damage
            };

            return cardCredentials;
        }

        private static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            //if string has length of one convert entire string
            return str.ToUpper();
        }
    }
}
