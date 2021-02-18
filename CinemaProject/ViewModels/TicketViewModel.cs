using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaProject.ViewModels
{
    public class TicketViewModel
    {
        public int SeatId { get; set; }
        public int MovieId { get; set; }
        public string MovieName{ get; set; }
    }
}