using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1HttpServer.Models
{
    class Round
    {
        public User User1 { get; }
        public User User2 { get; }
        public Card User1Card { get; }
        public Card User2Card { get; }
        public RoundLog RoundLog { get; } = new RoundLog();

        public double Card1CalculatedDamage { get; private set; } = 0;
        public double Card2CalculatedDamage { get; private set; } = 0;

        public Round(User user1, User user2, Card card1, Card card2)
        {
            User1 = user1;
            User2 = user2;

            User1Card = card1;
            User2Card = card2;

            Card1CalculatedDamage = User1Card.Damage;
            Card2CalculatedDamage = User2Card.Damage;
        }

        public void startRound(int roundNumber)
        {
            UserRoundLog user1RoundLog = new UserRoundLog(User1, User1Card);
            UserRoundLog user2RoundLog = new UserRoundLog(User2, User2Card);

            try
            {


                //log roundNumber
                RoundLog.RoundNumber = roundNumber;

                //base damage

                //get card weaknesses
                if (User1Card.isWeakAgainst(User2Card))
                {
                    user1RoundLog.IsWeak = true;
                    Card1CalculatedDamage = 0;
                }
                if (User2Card.isWeakAgainst(User1Card))
                {
                    user2RoundLog.IsWeak = true;
                    Card2CalculatedDamage = 0;
                }

                //get element multipliers if at least one spell is involved
                if (User1Card.CardType == CardType.spell || User2Card.CardType == CardType.spell)
                {
                    double card1DamageElementMultiplier = User1Card.getDamageElementMultiplier(User2Card.ElementType);
                    user1RoundLog.DamageElementMultiplier = card1DamageElementMultiplier;
                    Card1CalculatedDamage *= card1DamageElementMultiplier;


                    double card2DamageElementMultiplier = User2Card.getDamageElementMultiplier(User1Card.ElementType);
                    user2RoundLog.DamageElementMultiplier = card2DamageElementMultiplier;
                    Card2CalculatedDamage *= card2DamageElementMultiplier;
                }

                //draw
                if (Card1CalculatedDamage == Card2CalculatedDamage)
                {
                    RoundLog.RoundIsDraw = true;
                    return;
                }

                //calculate winner and looser
                User roundWinner = Card1CalculatedDamage > Card2CalculatedDamage ? User1 : User2;
                User roundLooser = roundWinner == User1 ? User2 : User1;

                Card roundWinnerCard = roundWinner == User1 ? User1Card : User2Card;
                Card roundLooserCard = roundLooser == User1 ? User1Card : User2Card;

                TransfereCard(roundLooserCard, roundWinner, roundLooser);

                RoundLog.RoundWinnerCardId = roundWinnerCard.Id;
                RoundLog.RoundWinner = roundWinner.Username;
            }
            finally
            {
                //log damage after round
                user1RoundLog.DealedDamage = Card1CalculatedDamage;
                user2RoundLog.DealedDamage = Card2CalculatedDamage;

                //log deck size after round
                user1RoundLog.CardsLeftInDeck = User1.Deck.Count;
                user2RoundLog.CardsLeftInDeck = User2.Deck.Count;

                //add UserRoundLogs to RoundLog
                RoundLog.UserRoundLogs = RoundLog.UserRoundLogs.Append(user1RoundLog);
                RoundLog.UserRoundLogs = RoundLog.UserRoundLogs.Append(user2RoundLog);
            }
        }

        private static void TransfereCard(Card card, User userToAdd, User userToRemove)
        {
            userToAdd.Deck.Add(card);
            userToRemove.Deck.Remove(card);
        }
    }
}
