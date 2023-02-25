using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;

namespace SWE1HttpServer.Models
{
    public abstract class Card
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double Damage { get; private set; }
        public CardType CardType { get; protected set; }
        public ElementType ElementType { get; private set; }

        public Card(string id, string name, double damage, ElementType elementType)
        {
            if (damage < 0)
            {
                throw new InvalidException();
            }

            Id = id;
            Name = name;
            Damage = damage;
            ElementType = elementType;
        }

        public abstract bool isWeakAgainst(Card enemyCard);

        public double getDamageElementMultiplier(ElementType enemyElement)
        {
            double multiplier = 1;
            switch (ElementType)
            {
                case ElementType.regular:
                    multiplier = compareElements(ElementType.water, ElementType.fire, enemyElement);
                    break;
                case ElementType.fire:
                    multiplier = compareElements(ElementType.regular, ElementType.water, enemyElement);
                    break;
                case ElementType.water:
                    multiplier = compareElements(ElementType.fire, ElementType.regular, enemyElement);
                    break;
            }
            return multiplier;
        }

        private static double compareElements(ElementType good, ElementType bad, ElementType enemyElement)
        {
            if (enemyElement == good) return 2;
            if (enemyElement == bad) return 0.5;
            return 1;
        }
    }
}
