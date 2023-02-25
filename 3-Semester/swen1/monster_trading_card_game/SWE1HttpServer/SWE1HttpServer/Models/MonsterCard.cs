using SWE1HttpServer.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models
{
    public class MonsterCard : Card
    {
        public MonsterCard(string id, MonsterName name, double damage, ElementType elementType) : base(id, name.ToString(), damage, elementType)
        {
            CardType = CardType.monster;
        }


        public override bool isWeakAgainst(Card enemyCard)
        {
            bool isWeak = false;
            if (!Enum.TryParse(Name, out MonsterName monsterName))
                throw new InternalServerErrorException();

            switch (monsterName)
            {
                case MonsterName.goblin:
                    if (enemyCard.Name == MonsterName.dragon.ToString()) isWeak = true;
                    break;
                case MonsterName.ork:
                    if (enemyCard.Name == MonsterName.wizzard.ToString()) isWeak = true;
                    break;
                case MonsterName.knight:
                    if (enemyCard.Name == SpellName.spell.ToString()) isWeak = true;
                    break;
                case MonsterName.dragon:
                    if (enemyCard.Name == MonsterName.elf.ToString() && enemyCard.ElementType == ElementType.fire) isWeak = true;
                    break;
            }
            return isWeak;
        }

    }
}
