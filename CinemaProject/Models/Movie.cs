using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.Models
{
    public class Movie
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Poster { get; set; }
        public int NumOfOrders { get; set; }
        public bool OnSale { get; set; }
        public int MinAge { get; set; }

    }
}