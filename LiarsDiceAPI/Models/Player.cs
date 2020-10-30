using System;
using System.Linq;
using System.Text.Json.Serialization;

namespace LiarsDiceAPI.Models
{
    public class Player
    {
        public string UserName { get;}
        public int DieLeft { get; } = Game.InitialDiceCount;

        public int DiceCount => DiceBag.Dice.Count();
        public DiceBag DiceBag { get; private set; }

        [JsonIgnore]
        public Guid UserId { get; }

        public Player(string userName)
        {
            UserName = userName;
            UserId = Guid.NewGuid();
            RollDiceBag();
        }

        public bool HasLost => DiceCount <= 1;

        public void RollDiceBag()
        {
            DiceBag = DieLeft;
        }
        
        public void RemoveDice()
        {
            DiceBag = DiceBag.Dice.Count() - 1;
        }
    }
}