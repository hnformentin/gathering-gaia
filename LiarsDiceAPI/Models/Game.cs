using System;
using System.Collections.Generic;
using System.Linq;

namespace LiarsDiceAPI.Models
{
    public class Game
    {
        public const int MaxPlayers = 4;
        public const int InitialDiceCount = 5;
        public GameRound CurrentRound { get; private set; }
        public string Name { get; }
        public GameStatus Status { get; private set; } = GameStatus.NotStarted;

        public Game(string gameName)
        {
            Id = Guid.NewGuid();
            Name = gameName;
            GameRegistry.Registry.Add(Id, this);
            Players = new Player[] { };
        }

        public Guid Id { get; }
        public Player[] Players { get; private set; }
        public IEnumerable<Player> ActivePlayers => Players.Where(player => !player.HasLost);
        public IEnumerable<Player> Losers => Players.Where(player => player.HasLost);

        public Player CurrentPlayer { get; private set; }
        public Bid LastBid { get; }

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
            if (Status == GameStatus.Running)
            {
                throw new InvalidOperationException("Cannot restart a running game");
            }
            if (Players.Length <= 1)
            {
                throw new InvalidOperationException("Requires more than 1 player");
            }

            Status = GameStatus.Running;
            CurrentPlayer = Players[new Random().Next(0, Players.Length - 1)];
            StartRoundWith(new Bid{  });
        }

        public void RollDice()
        {
            throw new NotImplementedException();
        }

        public void Call()
        {
            throw new NotImplementedException();
        }

        public void Bid(Die die, int nrOfDice)
        {
            throw new NotImplementedException();
        }

        public void StartRoundWith(Bid initialBid)
        {
            CurrentRound = new GameRound(initialBid, Id);
        }

        public static Game GetGameInstance(Guid guid)
        {
            // TODO: Handle Exceptions.
            return GameRegistry.Registry[guid];
        }

        public static IEnumerable<Player> GetActivePlayers(Guid guid)
        {
            return GetGameInstance(guid).ActivePlayers;
        }
    }
}