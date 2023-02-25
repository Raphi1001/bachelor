using SWE1HttpServer.Managers.Users;
using SWE1HttpServer.Managers.Cards;
using SWE1HttpServer.Managers.Decks;
using SWE1HttpServer.Managers.Stacks;
using SWE1HttpServer.Managers.Packages;
using SWE1HttpServer.Managers.Trades;
using SWE1HttpServer.Managers.Battles;
using SWE1HttpServer.DAL.UserRepository;
using SWE1HttpServer.DAL.CardRepository;
using SWE1HttpServer.DAL.PackageRepository;
using SWE1HttpServer.DAL.DeckRepository;
using SWE1HttpServer.DAL.TradeRepository;

namespace SWE1HttpServer.Managers
{
    public class BusinessManager
    {
        public UserManager UserManager { get; }
        public CardManager CardManager { get; }
        public DeckManager DeckManager { get; }
        public StackManager StackManager { get; }
        public PackageManager PackageManager { get; }
        public TradeManager TradeManager { get; }
        public BattleManager BattleManager { get; }




        public BusinessManager(IUserRepository userRepository, ICardRepository cardRepository, IPackageRepository packageRepository, IDeckRepository deckRepository, ITradeRepository tradeRepository)
        {
            UserManager = new UserManager(userRepository);
            CardManager = new CardManager();
            DeckManager = new DeckManager(cardRepository, deckRepository, tradeRepository);
            StackManager = new StackManager(cardRepository);
            PackageManager = new PackageManager(userRepository, cardRepository, packageRepository);
            TradeManager = new TradeManager(cardRepository, deckRepository, tradeRepository);
            BattleManager = new BattleManager(userRepository, deckRepository, cardRepository);   
        }

    }
}

