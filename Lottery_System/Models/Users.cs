using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lottery_System.Models
{
    public class Users
    {
        public long UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IsActive { get; set; }
        public DateTime CreatedDate { get; set; }        
    }
}