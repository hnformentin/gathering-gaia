using System;

namespace LiarsDiceAPI.Models
{
    public class GameRoundSummary
    {
        public Guid UserWithBid { get; set; }
        public Guid UserThatCalledLiar { get; set; }
        public bool CalledLiarSuccessfully { get; set; }
    }
}