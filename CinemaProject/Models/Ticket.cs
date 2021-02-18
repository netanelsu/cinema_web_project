using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CinemaProject.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int MovieScreeningId { get; set; }
        public int SeatNumber { get; set; }

        public virtual MovieScreen MovieScreening { get; set; }
    }
}