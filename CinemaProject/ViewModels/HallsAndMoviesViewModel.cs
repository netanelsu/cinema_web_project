using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CinemaProject.ViewModels
{
    public class HallsAndMoviesViewModel
    {
        public IEnumerable<SelectListItem> Halls { get; set; }
        public IEnumerable<SelectListItem> Movies { get; set; }
    }
}