using Npgsql;
using SWE1HttpServer.Models;
using SWE1HttpServer.Models.Logs;
using SWE1HttpServer.Models.Exceptions;
using SWE1HttpServer.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWE1HttpServer.Managers.Cards;
using System.Threading;

namespace SWE1HttpServer.DAL.CardRepository
{
    class DatabaseCardRepository : ICardRepository
    {
        private const string CreateTableCommand = @"
CREATE TABLE IF NOT EXISTS cards( 
    id VARCHAR PRIMARY KEY, 
    name VARCHAR, 
    damage REAL, 
    card_type card_type, 
    element_type element_type, 
    owner VARCHAR REFERENCES users(username) ON DELETE CASCADE)";

        private const string InsertCardsCommand = @"
INSERT INTO cards(id, name, damage, card_type, element_type) 
    VALUES 
        (@id0, @name0, @damage0, @card_type0, @element_type0), 
        (@id1, @name1, @damage1, @card_type1, @element_type1), 
        (@id2, @name2, @damage2, @card_type2, @element_type2),
        (@id3, @name3, @damage3, @card_type3, @element_type3),
        (@id4, @name4, @damage4, @card_type4, @element_type4)";

        private const string SelectDeckCardsByIdAndOwner = "SELECT id, name, damage, card_type, element_type FROM cards WHERE ( id=@0 OR id=@1 OR id=@2 OR id=@3 ) AND owner=@owner";
        private const string SelectCardsByOwnerCommand = "SELECT id, name, damage, element_type FROM cards WHERE owner=@owner";

        private const string SelectCardByIdCommand = "SELECT id, name, damage, element_type FROM cards WHERE id=@id";
        private const string SelectCardOwnerByIdCommand = "SELECT owner FROM cards WHERE id=@id";


        private const string UpdateCardOwnerCommand = "UPDATE cards SET owner=@owner WHERE id=@id";
        private const string SwapCardOwnerCommand = @"
UPDATE
    cards
SET
    owner = temp.new_owner
FROM
    (VALUES
        (@owner1, @owner2, @card1_id),
        (@owner2, @owner1, @card2_id)
    ) AS temp(past_owner, new_owner, card_id)
WHERE
    owner = temp.past_owner
    AND id = temp.card_id";


        private readonly NpgsqlConnection _connection;
        private Mutex Mutex { get; }

        public DatabaseCardRepository(NpgsqlConnection connection, Mutex mutex)
        {
            Mutex = mutex;
            _connection = connection;
            EnsureTables();
        }

        public bool InsertPackageCards(IList<Card> cards)
        {
            if (cards?.Count != (int)Constants.packageSize)
                throw new InvalidException();

            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(InsertCardsCommand, _connection);
                for (int i = 0; i < cards.Count; ++i)
                {
                    cmd.Parameters.AddWithValue("id" + i, cards[i].Id);
                    cmd.Parameters.AddWithValue("name" + i, cards[i].Name);
                    cmd.Parameters.AddWithValue("damage" + i, cards[i].Damage);
                    cmd.Parameters.AddWithValue("card_type" + i, cards[i].CardType);
                    cmd.Parameters.AddWithValue("element_type" + i, cards[i].ElementType);
                }
                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }

            }
            catch (PostgresException)
            {
                // this might happen, if one cards already exists (constraint violation)
                throw new ConflictException();
            }

            return affectedRows > 0;
        }

        public bool UpdateCardOwner(string cardId, string username)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(UpdateCardOwnerCommand, _connection);
                cmd.Parameters.AddWithValue("id", cardId);
                cmd.Parameters.AddWithValue("owner", username);
                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }

            }
            catch (PostgresException)
            {
                // this might happen, if the card already exists (constraint violation)
                // we just catch it and keep affectedRows at zero
            }

            return affectedRows > 0;
        }

        public bool SwapCardOwner(string card1Id, string card2Id, string card1Owner, string card2Owner)
        {
            var affectedRows = 0;
            try
            {
                using var cmd = new NpgsqlCommand(SwapCardOwnerCommand, _connection);
                cmd.Parameters.AddWithValue("owner1", card1Owner);
                cmd.Parameters.AddWithValue("owner2", card2Owner);
                cmd.Parameters.AddWithValue("card1_id", card1Id);
                cmd.Parameters.AddWithValue("card2_id", card2Id);

                try
                {
                    Mutex.WaitOne();
                    affectedRows = cmd.ExecuteNonQuery();
                }
                finally { Mutex.ReleaseMutex(); }

            }
            catch (PostgresException)
            {
                // this might happen, if the card already exists (constraint violation)
                // we just catch it and keep affectedRows at zero
            }

            return affectedRows > 0;
        }

        public Card GetCardById(string cardId)
        {
            Card card = null;

            using (var cmd = new NpgsqlCommand(SelectCardByIdCommand, _connection))
            {
                cmd.Parameters.AddWithValue("id", cardId);

                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take the first row, if any
                    if (reader.Read())
                    {
                        card = ReadCard(reader);
                    }
                }
                finally { Mutex.ReleaseMutex(); }
            }

            return card;
        }

        public string GetCardOwnerById(string cardId)
        {
            string cardOwner = null;

            using (var cmd = new NpgsqlCommand(SelectCardOwnerByIdCommand, _connection))
            {
                cmd.Parameters.AddWithValue("id", cardId);
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();

                    // take the first row, if any
                    if (reader.Read())
                    {
                        cardOwner = reader["owner"].ToString();
                    }
                }
                finally { Mutex.ReleaseMutex(); }
            }

            return cardOwner;
        }


        private void EnsureTables()
        {
            using var cmd = new NpgsqlCommand(CreateTableCommand, _connection);
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Card> GetAllCards(string username)
        {
            IEnumerable<Card> cards = new List<Card>();

            using (var cmd = new NpgsqlCommand(SelectCardsByOwnerCommand, _connection))
            {
                cmd.Parameters.AddWithValue("owner", username);
                try
                {
                    Mutex.WaitOne();
                    using var reader = cmd.ExecuteReader();
                    // take all rows, if any
                    while (reader.Read())
                    {
                        Card currentCard = ReadCard(reader);
                        cards = cards.Append(currentCard);
                    }
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }

            return cards;
        }

        public IList<Card> GetAllDeckCards(string username, IList<string> cardIds)
        {
            if (cardIds?.Count != (int)Constants.deckSize)
                throw new InternalServerErrorException();


            IList<Card> deck = new List<Card>();

            using (var cmd = new NpgsqlCommand(SelectDeckCardsByIdAndOwner, _connection))
            {
                cmd.Parameters.AddWithValue("owner", username);
                for (int i = 0; i < cardIds.Count; ++i)
                {
                    cmd.Parameters.AddWithValue(i.ToString(), cardIds[i]);
                }
                try
                {
                    using var reader = cmd.ExecuteReader();

                    // take all rows, if any
                    while (reader.Read())
                    {
                        Card currentCard = ReadCard(reader);
                        deck.Add(currentCard);
                    }
                }
                finally { Mutex.ReleaseMutex(); }
            }
            return deck;
        }


        private Card ReadCard(IDataRecord record)
        {
            try
            {
                return CardManager.CreateCard(record["id"].ToString(), record["element_type"].ToString(), record["name"].ToString(), Convert.ToDouble(record["damage"]));
            }
            catch (InvalidException)
            {
                throw new InternalServerErrorException();
            }

        }
    }
}
