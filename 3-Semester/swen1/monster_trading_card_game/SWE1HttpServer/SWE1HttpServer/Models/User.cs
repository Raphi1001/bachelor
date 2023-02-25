using SWE1HttpServer.Core.Authentication;
using SWE1HttpServer.Managers;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Models.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models
{
    public class User : IIdentity
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; } = "";
        public string Bio { get; private set; } = "";
        public string Image { get; private set; } = "";
        public int Coins { get; private set; } = (int)Constants.userStartingCoinCount > 0 ? (int)Constants.userStartingCoinCount : 0;
        public int EloScore { get; private set; } = (int)Constants.userStartingEloScore > 0 ? (int)Constants.userStartingEloScore : 0;
        public int PlayedGames { get; private set; } = 0;
        public int WonGames { get; private set; } = 0;
        public int LostGames { get; private set; } = 0;

        public string Token { get; private set; }

        public List<Card> Stack { get; } = new List<Card>();
        public List<Card> DeckCards { get; } = new List<Card>();
        public List<Card> Deck { get; } = new List<Card>();


        public User(string username, string password)
        {
            setUserCredentials(username, password);
        }

        public User(string username, string password, int coins, int eloScore, int playedGames, int wonGames, int lostGames)
        {
            PlayedGames = playedGames > 0 ? playedGames : 0;
            WonGames = wonGames > 0 ? wonGames : 0;
            LostGames = lostGames > 0 ? lostGames : 0;
            Coins = coins > 0 ? coins : 0;
            EloScore = eloScore > 0 ? eloScore : 0;
            setUserCredentials(username, password);
        }

        private static string getUserToken(string username)
        {
            return username + "-mtcgToken";
        }

        private void setUserCredentials(string username, string password)
        {
            StringValidator.Validate(username);
            StringValidator.Validate(password);

            Username = username;
            Password = password;
            Token = getUserToken(Username);
        }

        public void SetUserData(string name, string bio, string image)
        {
            StringValidator.ValidateEmpty(name);
            StringValidator.ValidateEmpty(bio);
            StringValidator.ValidateEmpty(image);

            Name = name;
            Bio = bio;
            Image = image;
        }

        public void AcquirePackage()
        {
            int newCoins = Coins - (int)Constants.packagePrice;
            Coins = newCoins > 0 ? newCoins : 0;
        }
        public void Play()
        {
            ++PlayedGames;
        }
        public void WinBattle()
        {
            int newEloScore = EloScore + (int)Constants.EloScoreLossReduction;
            Coins = newEloScore > 0 ? newEloScore : 0;
            ++WonGames;
        }

        public void LooseBattle()
        {
            int newEloScore = EloScore - (int)Constants.EloScoreLossReduction;
            Coins = newEloScore > 0 ? newEloScore : 0;
            ++LostGames;
        }

        public void resetDeck()
        {
            Deck.Clear();
            Deck.AddRange(DeckCards);
        }

        public Card GetRandomDeckCard()
        {
            if (Deck.Count < 1) throw new InvalidException();

            int index = Program.rand.Next(Deck.Count);

            return Deck[index];
        }
    }
}
