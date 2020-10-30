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
            _game.ActivePlayers.ToList().ForEach(x =>
            {
                x.RollDiceBag(); 
            });
        }

        public Dictionary<int,int> CountDice()
        {
            // kunne brukt array, men ettersom terninger ikke er 0-basert som arrays, så blir det tydeligere med dictionary.
            var result = new Dictionary<int,int>();
            var dice = _game.ActivePlayers.ToList().SelectMany(x => x.DiceBag.Dice);
            dice.ToList().ForEach(x =>
            {
                if (result.ContainsKey(x))
                {
                    result.Add(x,0);
                }
                result[x]++;
            });
            return result;
        }

        public void RaiseBid(Bid newBid)
        {
            if (newBid.Die > CurrentBid.Die || newBid.NrOfDice > CurrentBid.NrOfDice)
            {
                CurrentBid = newBid;    
            }
            else
            {
                throw new ArgumentException("Cannot place a bid that does not increase die value or number of dice.");
            }
            
        }

        public void CallLiar()
        {
            var totalDiceCount = CountDice();
            var numBidDice = totalDiceCount[CurrentBid.Die];
            if (numBidDice > CurrentBid.NrOfDice)
            {
                _game.EndRound();
            }

        }
    }
}