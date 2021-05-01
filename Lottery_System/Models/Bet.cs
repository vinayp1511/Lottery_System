using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lottery_System.Models
{
    public class Bet
    {
        public long BetID { get; set; }
        public long UserID { get; set; }
        public int BetNumber { get; set; }
        public decimal BetAmount { get; set; }
        public decimal TotalBetAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}