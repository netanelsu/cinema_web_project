using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CinemaProject.Models
{
    public class SeatReserved
    {
        public int Id { get; set; }
        public int Seat { get; set; }

        public virtual MovieScreen MovieScreening { get; set; }
        public Nullable<int> MovieScreeningId { get; set; }
    }
}