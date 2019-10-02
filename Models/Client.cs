using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompoShop.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}