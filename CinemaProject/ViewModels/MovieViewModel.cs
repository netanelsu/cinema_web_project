using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CinemaProject.ViewModels
{
    public class MovieViewModel
    {
        public int MoviePresentId { get; set; }
        [Display(Name = "Name")]
        public string MovieName { get; set; }
        public DateTime Date { get; set; }
        public int HallId { get; set; }
        public string Poster { get; set; }
        [Display(Name = "minimum age")]
        public int MinAge { get; set; }

        public double Price { get; set; }
        public bool onSale { get; set; }
        public string Category { get; set; }

    }
}