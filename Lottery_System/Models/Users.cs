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
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public long CountryId { get; set; }
        public string Country { get; set; }
        public long StateId { get; set; }
        public string State { get; set; }
        public string Hobbie { get; set; }
        public string Gender { get; set; }
    }
}