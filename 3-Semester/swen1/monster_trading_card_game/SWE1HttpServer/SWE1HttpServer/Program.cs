using SWE1HttpServer.Core.Request;
using SWE1HttpServer.Core.Routing;
using SWE1HttpServer.Core.Server;
using SWE1HttpServer.DAL;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.RouteCommands.Messages;
using SWE1HttpServer.RouteCommands.Users;
using SWE1HttpServer.RouteCommands.Stack;
using SWE1HttpServer.RouteCommands.Package;
using SWE1HttpServer.RouteCommands.Deck;
using SWE1HttpServer.RouteParsers;
using System;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using SWE1HttpServer.IdentityProvider;
using SWE1HttpServer.RouteCommands.Trades;
using SWE1HttpServer.RouteCommands.Battles;

namespace SWE1HttpServer
{
    public class Program
    {
        public static readonly Random rand = new();
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server...");

            var db = new Database("Host=localhost;Port=5432;Username=postgres;Password=admin;Database=mtcg");

            var businessManager = new BusinessManager(db.UserRepository, db.CardRepository, db.PackageRepository, db.DeckRepository, db.TradeRepository);
            
            var identityProvider = new UserIdentityProvider(db.UserRepository);
            var routeParser = new VarRouteParser();

            var router = new Router(routeParser, identityProvider);
            RegisterRoutes(router, businessManager);

            var mtcgServer = new MonsterTradingCardGameServer(IPAddress.Any, 10001, router);
            mtcgServer.Start();
        }

        private static void RegisterRoutes(Router router, BusinessManager businessManager)
        {
            /* USER ROUTES */

            //get user token
            router.AddRoute(HttpMethod.Post, "/sessions", (r, p) => new LoginCommand(businessManager.UserManager, Deserialize<UserCredentialsLog>(r.Payload)));
            //register user
            router.AddRoute(HttpMethod.Post, "/users", (r, p) => new RegisterCommand(businessManager.UserManager, Deserialize<UserCredentialsLog>(r.Payload)));
            //show user data
            router.AddProtectedRoute(HttpMethod.Get, "/users/{var}", (r, p) => new ShowUserDataCommand(businessManager.UserManager, p["var"].ToString()));
            //edit user data    
            router.AddProtectedRoute(HttpMethod.Put, "/users/{var}", (r, p) => new EditUserDataCommand(businessManager.UserManager, p["var"].ToString(), Deserialize<UserDataLog>(r.Payload)));
            //user stats
            router.AddProtectedRoute(HttpMethod.Get, "/stats", (r, p) => new ShowUserStatsCommand(businessManager.UserManager));
            
            // scoreboard
            router.AddProtectedRoute(HttpMethod.Get, "/score", (r, p) => new ShowScoreboardCommand(businessManager.UserManager));

            /* PACKAGE ROUTES */

            //add package (admin)
            router.AddProtectedRoute(HttpMethod.Post, "/packages", (r, p) => new AddPackageCommand(businessManager.PackageManager, Deserialize<List<CardLog>>(r.Payload)));
            //buy package
            router.AddProtectedRoute(HttpMethod.Post, "/transactions/packages", (r, p) => new AcquirePackageCommand(businessManager.PackageManager));

            /* STACK ROUTES */

            //show stack
            router.AddProtectedRoute(HttpMethod.Get, "/cards", (r, p) => new ListStackCommand(businessManager.StackManager));

            /* DECK ROUTES */

            //Show deck
            router.AddProtectedRoute(HttpMethod.Get, "/deck", (r, p) => new ShowDeckCommand(businessManager.DeckManager));
            //Show deck plain
            router.AddProtectedRoute(HttpMethod.Get, "/deck\\?format={var}", (r, p) => new ShowDeckCommand(businessManager.DeckManager, p["var"]));
            //Update Deck
            router.AddProtectedRoute(HttpMethod.Put, "/deck", (r, p) => new ConfigureDeckCommand(businessManager.DeckManager, Deserialize<List<string>>(r.Payload)));

            /* BATTLE ROUTES */
            
            //search for battle
            router.AddProtectedRoute(HttpMethod.Post, "/battles", (r, p) => new QueueForBattleCommand(businessManager.BattleManager));

            /* TRADING ROUTES */
            
            //check trading deals
            router.AddProtectedRoute(HttpMethod.Get, "/tradings", (r, p) => new ShowTradesCommand(businessManager.TradeManager));
            //post trading deal
            router.AddProtectedRoute(HttpMethod.Post, "/tradings", (r, p) => new CreateTradeCommand(businessManager.TradeManager, Deserialize<TradeLog>(r.Payload)));
            //accept trading deal
            router.AddProtectedRoute(HttpMethod.Post, "/tradings/{var}", (r, p) => new AcceptTradeCommand(businessManager.TradeManager, p["var"], Deserialize<string>(r.Payload)));
            //delete trading deal
            router.AddProtectedRoute(HttpMethod.Delete, "/tradings/{var}", (r, p) => new DeleteTradeCommand(businessManager.TradeManager, p["var"]));

        }

        private static T Deserialize<T>(string payload) where T : class
        {
            var deserializedData = JsonConvert.DeserializeObject<T>(payload);

            return deserializedData;
        }
    }
}

