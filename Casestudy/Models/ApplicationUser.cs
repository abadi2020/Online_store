using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casestudy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Age { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string Mailcode { get; set; }
        public string Country { get; set; }
        public string CreditcardType { get; set; }
        public string Region { get; set; }
    }
}
