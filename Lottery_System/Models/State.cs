using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lottery_System.Models
{
    public class State
    {
        public long StateId { get; set; }
        public long CountryId { get; set; }
        public string StateName { get; set; }
    }
}