using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaProject.ViewModels
{
    public class BuyTicketViewModel
    {
        public MovieViewModel Movie { get; set; }
        public List<SelectListItem> Seats { get; set; }
        public string Error { get; set; }
    }
}