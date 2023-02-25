using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models
{
    public class SpellCard : Card
    {
        public SpellCard(string id, SpellName name, double damage, ElementType elementType) : base(id, name.ToString(), damage, elementType)
        {
            CardType = CardType.spell;
        }

        public override bool isWeakAgainst(Card enemyCard)
        {
            bool isWeak = false;
            if (!Enum.TryParse(Name, out SpellName spellName))
                throw new InternalServerErrorException();

            switch (spellName)
            {
                case SpellName.spell:
                    if (enemyCard.Name == MonsterName.kraken.ToString()) isWeak = true;
                    break;
            }
            return isWeak;
        }
    }
}
