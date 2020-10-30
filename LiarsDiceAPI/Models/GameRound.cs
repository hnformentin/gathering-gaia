using System;
using System.Collections.Generic;
using System.Linq;

namespace LiarsDiceAPI.Models
{
    public class GameRound
    {
        private readonly Game _game;

        // What is the current bid?
        public Bid CurrentBid { get; private set; }

        public GameRound(Game game)
        {
            _game = game;
            CurrentBid = default;
            RollDiceForAllPlayers();
        }

        private void RollDiceForAllPlayers()
        {
            _game.ActivePlayers.ToList().ForEach(x => { x.RollDiceBag(); });
        }

        public Dictionary<int, int> CountDice()
        {
            // kunne brukt array, men ettersom terninger ikke er 0-basert som arrays, så blir det tydeligere med dictionary.
            var result = new Dictionary<int, int>();
            var dice = _game.ActivePlayers.ToList().SelectMany(x => x.DiceBag.Dice);
            dice.ToList().ForEach(x =>
            {
                if (result.ContainsKey(x))
                {
                    result.Add(x, 0);
                }

                result[x]++;
            });
            return result;
        }

        public void RaiseBid(Bid newBid)
        {
            if (newBid.Die == 1 || newBid.Die == 6 || newBid.Die <= CurrentBid.Die ||
                newBid.NrOfDice <= CurrentBid.NrOfDice)
            {
                throw new ArgumentException("Cannot place a bid that does not increase die value or number of dice.");
            }

            CurrentBid = newBid;
        }

        public void CallLiar(Guid currentPlayerUserId)
        {
            var totalDiceCount = CountDice();
            var numBidDice = totalDiceCount[CurrentBid.Die] + totalDiceCount[1];

            _game.EndRound(new GameRoundSummary()
            {
                UserWithBid = CurrentBid.UserId,
                UserThatCalledLiar = currentPlayerUserId,
                CalledLiarSuccessfully = CurrentBid.NrOfDice < numBidDice
            });
        }
    }
}

