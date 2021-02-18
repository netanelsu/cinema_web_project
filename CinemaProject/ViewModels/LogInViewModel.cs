using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.ViewModels
{
    public class LogInViewModel
    {
        [Key]
        public String UserName { get; set; }
        public String Password { get; set; }
    }
}