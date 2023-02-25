using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models
{
    public class Battle
    {
        public User User1 { get; }
        public User User2 { get; }
        public User BattleWinner { get; private set; } = null;
        public User BattleLooser { get; private set; } = null;

        public BattleLog BattleLog { get; } = new BattleLog();

        public Battle(User user1, User user2)
        {
            if (user1 == user2)
                throw new ForbiddenException();

            User1 = user1;
            User2 = user2;
        }

        public void startBattle()
        {
            if (User1.DeckCards?.Count != 4 || User2.DeckCards?.Count != 4)
                throw new ForbiddenException();
            
            User1.Play();
            User2.Play();
            
            //create logs of user decks
            BattleLog.DeckLogs = BattleLog.DeckLogs.Append(new DeckLog(User1));
            BattleLog.DeckLogs = BattleLog.DeckLogs.Append(new DeckLog(User2));

            User1.resetDeck();
            User2.resetDeck();

            for (int i = 1; i <= 100; ++i)
            {
                Card user1Card = User1.GetRandomDeckCard();
                Card user2Card = User2.GetRandomDeckCard();
                Round round = new Round(User1, User2, user1Card, user2Card);
                round.startRound(i);


                BattleLog.RoundLogs = BattleLog.RoundLogs.Append(round.RoundLog);

                //Check if someone has won the battle
                if (User1.Deck.Count < 1)
                    setBattleResult(User2, User1, i);
                else if (User2.Deck.Count < 1)
                    setBattleResult(User1, User2, i);
                else
                    continue;

                return;
            }

            BattleLog.BattleIsDraw = true;

        }
        private void setBattleResult(User battleWinner, User battleLooser, int roundCount)
        {
            BattleWinner = battleWinner;
            BattleLooser = battleLooser;
            BattleWinner?.WinBattle();
            BattleLooser?.LooseBattle();
            BattleLog.BattleWinner = BattleWinner?.Username;

            BattleLog.RoundCount = roundCount;
        }
    }
}
