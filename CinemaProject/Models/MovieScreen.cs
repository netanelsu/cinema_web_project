using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CinemaProject.Models
{
    public class MovieScreen
    {
        public MovieScreen()
        {
            SeatsTaken = new List<SeatReserved>();
        }
        public int Id { get; set; }
        public int HallId { get; set; }
        public int MovieId { get; set; }
        public DateTime Date { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual ICollection<SeatReserved> SeatsTaken { get; set; }
    }
}