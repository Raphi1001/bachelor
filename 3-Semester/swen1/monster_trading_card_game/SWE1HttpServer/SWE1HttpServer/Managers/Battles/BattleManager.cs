using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.DAL.DeckRepository;
using SWE1HttpServer.DAL.UserRepository;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;

namespace SWE1HttpServer.Managers.Battles
{
    public class BattleManager : IBattleManager
    {
        private IUserRepository UserRepository { get; }
        private IDeckRepository DeckRepository { get; }
        private ICardRepository CardRepository { get; }

        private static Queue<(User, ManualResetEvent, Action<BattleLog>)> BattleQueue = new Queue<(User, ManualResetEvent, Action<BattleLog>)>();
        public BattleManager(IUserRepository userRepository, IDeckRepository deckRepository, ICardRepository cardRepository)
        {
            UserRepository = userRepository;
            DeckRepository = deckRepository;
            CardRepository = cardRepository;
        }

        public BattleLog QueueForBattle(User user)
        {
            user = UserRepository.GetUserData(user.Token);
            IList<string> deckIds = DeckRepository.GetUserDeckIds(user.Username);

            if (deckIds?.Count != (int)Constants.deckSize)
            {
                throw new ForbiddenException();
            }

            IList<Card> deck = CardRepository.GetAllDeckCards(user.Username, deckIds);
            user.DeckCards.AddRange(deck);

            if (BattleQueue.Count == 0)
            {
                ManualResetEvent manualResetEvent = new ManualResetEvent(false);

                BattleLog battleLog = new BattleLog();

                Action<BattleLog> battleLogAction = new Action<BattleLog>(a => { battleLog = a; });

                BattleQueue.Enqueue((user, manualResetEvent, battleLogAction));

                manualResetEvent.WaitOne();

                return battleLog;
            }
            else
            {
                (User enemy, ManualResetEvent manualResetEvent, Action<BattleLog> battleLogAction) = BattleQueue.Dequeue();

                Battle battle = new Battle(user, enemy);

                battle.startBattle();

                UserRepository.UpdateUserScore(user.Username, user.EloScore, user.PlayedGames, user.WonGames, user.LostGames);
                UserRepository.UpdateUserScore(enemy.Username, enemy.EloScore, enemy.PlayedGames, enemy.WonGames, enemy.LostGames);

                battleLogAction(battle.BattleLog);

                manualResetEvent.Set();

                return battle.BattleLog;
            }
        }
    }
}
