using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CinemaProject.Dal;
using CinemaProject.Models;
using CinemaProject.ViewModels;

namespace CinemaProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminHomePage()
        {
            var Context = new MoviesDBMapping();
            var movies = Context.Movies.ToList();
            return View(movies);
        }

        [HttpGet]
        public ActionResult AddMovie() // טופס הוספת סרט
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddMovie(Movie myMovie) // שליחת המידע בפוסט
        {
            if (myMovie != null)
            {
                var movieContext = new MoviesDBMapping();
                movieContext.Movies.Add(myMovie);
                movieContext.SaveChanges();
            }

            return RedirectToAction("AdminHomePage");
        }


        public ActionResult RemoveMovie(int id)
        {
            if (id > 0)
            {
                var movieContext = new MoviesDBMapping();
                var myMovie = movieContext.Movies.FirstOrDefault(m => m.Id == id);
                if (myMovie != null)
                {
                    movieContext.Movies.Remove(myMovie);
                    movieContext.SaveChanges();
                }
            }
            return RedirectToAction("AdminHomePage");
        }
        public ActionResult ManageHall()
        {
            var context = new MoviesDBMapping();
            var h = context.Hall.ToList();
            return View(h);
        }

        public ActionResult AddHall()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddHall(Hall newHall)
        {
            var movieContext = new MoviesDBMapping();
            movieContext.Hall.Add(newHall);
            movieContext.SaveChanges();
            return RedirectToAction("Halls");
        }

        public ActionResult Halls()
        {
            var movieContext = new MoviesDBMapping();
            var HallsList = movieContext.Hall.ToList();
            return View(HallsList);
        }

        public ActionResult MoviesScreenings()
        {
            var movieContext = new MoviesDBMapping();
            var moviesList = movieContext.MoviesList.Include("Movie").Include("Hall").Include("SeatsTaken").ToList();
            return View(moviesList);
        }

        public ActionResult AddMoviesToScreenings()
        {
            var Context = new MoviesDBMapping();
            HallsAndMoviesViewModel viewModel = new HallsAndMoviesViewModel();

            viewModel.Halls = Context.Hall.Select(h =>
                 new SelectListItem
                 {
                     Text = h.Id.ToString(),
                     Value = h.Id.ToString()
                 });

            viewModel.Movies = Context.Movies.Select(m =>
                 new SelectListItem
                 {
                     Text = m.Name,
                     Value = m.Id.ToString()
                 });

            ViewData["data"] = viewModel;

            return View();
        }

        [HttpPost]
        public ActionResult AddMoviesToScreenings(MovieScreen movie)
        {
            var context = new MoviesDBMapping();
            var Screenings = context.MoviesList.ToList();
            
            foreach(MovieScreen m in Screenings)
            {
                if(m.HallId == movie.HallId && m.Date == movie.Date)
                {
                    return View("ErrorHallTaken");
                }
            }
            if (movie != null)
            {
                var movieContext = new MoviesDBMapping();
                movieContext.MoviesList.Add(movie);
                movieContext.SaveChanges();
            }
            return RedirectToAction("MoviesScreenings");
        }

        public ActionResult ChangePrice(int id)
        {
            var Context = new MoviesDBMapping();
            var movie = Context.Movies.FirstOrDefault(m => m.Id == id);
            return View(movie);
        }
        [HttpPost]
        public ActionResult ChangePrice(Movie movie)
        {
            var Context = new MoviesDBMapping();
            var m = Context.Movies.Find(movie.Id);
            m.Price = movie.Price;
            Context.SaveChanges();
            return RedirectToAction("AdminHomePage");
        }
         public ActionResult ChangeSeatNumber(int id) {

            var Context = new MoviesDBMapping();
            var Hall = Context.Hall.FirstOrDefault(m => m.Id == id);
            return View(Hall);

        }

        [HttpPost]
        public ActionResult ChangeSeatNumber(Hall hall)
        {

            var Context = new MoviesDBMapping();
            var h = Context.Hall.Find(hall.Id);
            h.Seats = hall.Seats;
            Context.SaveChanges();
            return RedirectToAction("AdminHomePage");

        }

        public ActionResult Screenings()
        {
            var movieContext = new MoviesDBMapping();
            var ml = movieContext.MoviesList.ToList();
               
               
            return View(ml);
        }
        public ActionResult ChangeHall(int id)
        {
            var movieContext = new MoviesDBMapping();
            var screen = movieContext.MoviesList.FirstOrDefault(m => m.Id == id);
            return View(screen);
        }
        
        [HttpPost]
        public ActionResult ChangeHall(MovieScreen screen)
        {
            var movieContext = new MoviesDBMapping();
            var Screenings = movieContext.MoviesList.ToList();

            foreach (MovieScreen m in Screenings)
            {
                if (m.HallId == screen.HallId && m.Date ==screen.Date)
                {
                    return View("ErrorHallTaken");
                }
            }
            var s = movieContext.MoviesList.Find(screen.Id);
            s.HallId = screen.HallId;
            movieContext.SaveChanges();
            return RedirectToAction("AdminHomePage");
        }
        

    }
}