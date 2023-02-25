using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models
{

    public enum Constants
    {
        userStartingCoinCount = 20,
        packagePrice = 5,
        userStartingEloScore = 100,
        EloScoreLossReduction = 5,
        EloScoreWinIncrease = 3,
        packageSize = 5,    // need to adjust database aswell if packageSize is changed 
        deckSize = 4        // need to adjust database aswell if deckSize is changed 
    }

    public enum ResponseDeckFormatType
    {
        json,
        plain
    }

    public enum CardType
    {
        monster,
        spell
    }

    public enum ElementType
    {
        regular,
        fire,
        water
    }
    public enum MonsterName
    {
        troll,
        goblin,
        dragon,
        wizzard,
        ork,
        knight,
        kraken,
        elf
    }

    public enum SpellName
    {
        spell
    }

    public enum Effectiveness
    {
        regular,
        effective,
        ineffective
    }
}
