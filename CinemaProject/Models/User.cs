using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public String UserType { get; set; }
        //public List<Ticket> Tickets { get; set; }
    }
}