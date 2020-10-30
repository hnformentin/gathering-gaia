using System;
using System.Collections.Generic;
using System.Linq;

namespace LiarsDiceAPI.Models
{
    public class Game
    {
        public const int MaxPlayers = 4;
        public const int DefaultDice = 5;
        public GameRound CurrentRound { get; private set; }
        public string Name { get; }
        public GameStatus Status { get; private set; } = GameStatus.NotStarted;

        public Game(string gameName)
        {
            Id = Guid.NewGuid();
            Name = gameName;
            GameRegistry.Registry.Add(Id, this);
            Round = 0;
            Players = new Player[] { };
        }

        public Guid Id { get; }
        public int Round { get; }
        public Player[] Players { get; private set; }
        public IEnumerable<Player> ActivePlayers => Players.Where(player => !player.HasLost);
        public IEnumerable<Player> Losers => Players.Where(player => player.HasLost);
        
        public Player CurrentPlayer { get; }
        public Bid CurrentBid { get; }

        public bool HasStarted => Round > 0;

        public Player JoinGame(string userName)
        {
            switch (Status)
            {
                case GameStatus.Running:
                    throw new InvalidOperationException("Cannot join game that has already started");
                case GameStatus.Finished:
                    throw new InvalidOperationException("Cannot join game that is finished");
            }

            if (Players.Length >= MaxPlayers)
            {
                throw new InvalidOperationException("Max number of players");
            }
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidOperationException("Username cannot be empty");
            }
            if (Players.Any(player => userName.Equals(player.UserName, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new InvalidOperationException("User with same name already registered");
            }

            var player = new Player(userName);
            Players = Players.Append(player).ToArray();
            return player;
        }

        public void StartGame()
        {
            throw new NotImplementedException();
        }

        public void RollDice()
        {
            throw new NotImplementedException();
        }

        public void Call()
        {
            if (Status == GameStatus.Running)
            {
                CurrentRound.CallLiar();
            } 
        }

        public void Bid(Die die, int nrOfDice)
        {
            if (Status == GameStatus.Running)
            {
                CurrentRound.RaiseBid(new Bid(die, nrOfDice, CurrentPlayer.UserId));
            }
        }

        public void StartRound()
        {
            CurrentRound = new GameRound(this);
        }

        public void EndRound()
        {
            throw new NotImplementedException();
        }
    }
}