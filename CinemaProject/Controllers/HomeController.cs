using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CinemaProject.Dal;
using CinemaProject.ViewModels;
using CinemaProject.Models;

namespace CinemaProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            var movieContext = new MoviesDBMapping();
            if (Session["start"] as string != "false")
            {
                var tickets = movieContext.Tickets.ToList();
                var seats = movieContext.SeatsTaken.ToList();
                if (seats != null)
                {
                    foreach (SeatReserved s in seats)
                    {
                        movieContext.SeatsTaken.Remove(s);
                        movieContext.SaveChanges();
                    }
                }
                if (tickets != null)
                {
                    foreach (Ticket t in tickets)
                    {
                        var ScreenOSeats = movieContext.MoviesList.Find(t.MovieScreeningId).SeatsTaken;
                        var SeatO = new SeatReserved();
                        SeatO.MovieScreeningId = t.MovieScreeningId;
                        SeatO.Seat = t.SeatNumber;
                        ScreenOSeats.Add(SeatO);
                        movieContext.SaveChanges();
                    }
                }
            }
            Session["start"] = "false";


            var ml = movieContext.MoviesList.ToList();
                //.Include("Movies")
                //.Include(b=>b.Hall)
                //.Take(10).OrderByDescending(m => m.Date);

            var moviesVM = ml.Select(m => new MovieViewModel
            {
                MoviePresentId = m.Id,
                MovieName = m.Movie.Name,
                Date = m.Date,
                HallId = m.HallId,
                MinAge = m.Movie.MinAge,
                Poster = m.Movie.Poster,
                Price = m.Movie.Price,
                onSale =m.Movie.OnSale,
                Category = m.Movie.Category

            }).ToList();

            return View(moviesVM);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            var movieContext = new MoviesDBMapping();

            user.UserType = "Regular";
            movieContext.User.Add(user);
            movieContext.SaveChanges();

            return View();
        }

        public ActionResult SignIn()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(LogInViewModel user)
        {
            var movieContext = new MoviesDBMapping();
            var _user = movieContext.User.FirstOrDefault(m => m.Password == user.Password && m.UserName == user.UserName);

            if (_user == null) // user not exists
            {
                return RedirectToAction("Index");
            }

            Session["userName"] = user.UserName;
            Session["loggedin"] = "true";

            if (_user.UserType == "Admin") // user admin
            {
                Session["isAdmin"] = true;
                return RedirectToAction("AdminHomePage", "Admin");
            }


            return RedirectToAction("Index");

        }


        public ActionResult BuyTicket(int id)
        {
            var Context = new MoviesDBMapping();
            var Screening = Context.MoviesList
                .Include("Hall")
                .Include("SeatsTaken")
                .FirstOrDefault(m => m.Id == id);

            var viewModel = new BuyTicketViewModel();
            if (Screening == null) // movie dosent exist
            {
                viewModel.Error = "No movie found";
            }
            else if (Screening.Hall.Seats == Screening.SeatsTaken.Count()) // no seats left
            {
                viewModel.Error = "No seats available";
            }
            else // order ticket
            {
                viewModel.Movie = new MovieViewModel
                {
                    MoviePresentId = Screening.Id,
                    Date = Screening.Date,
                    HallId = Screening.HallId,
                    MinAge = Screening.Movie.MinAge,
                    MovieName = Screening.Movie.Name
                };

                viewModel.Seats = new List<SelectListItem>();
                for (int i = 1; i <= Screening.Hall.Seats; i++)
                {
                    if (!Screening.SeatsTaken.Any(a => a.Seat == i))
                    {
                        SelectListItem sli = new SelectListItem { Text = i.ToString(), Value = i.ToString() };
                        viewModel.Seats.Add(sli);
                    }
                }
            }


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult BuyTicket()
        {
            var movieId = Request.Form["movieId"];
            var movieName = Request.Form["movieName"];
            var seat = Request.Form["Seat"];
            
            // if session exists
            var MoviePasses = Session["Tickets"] as List<TicketViewModel>;
            if (MoviePasses == null)
            {
                MoviePasses = new List<TicketViewModel>();
            }
            if (movieId != null && seat != null )
            {
                var context = new MoviesDBMapping();
                var screeningOSeats = context.MoviesList.Find(int.Parse(movieId)).SeatsTaken;
                var SeatO = new SeatReserved();
                SeatO.Seat = int.Parse(seat);
                screeningOSeats.Add(SeatO);
                context.SaveChanges();
                // add to session new movies
                
                MoviePasses.Add(new TicketViewModel { MovieId = int.Parse(movieId), SeatId = int.Parse(seat), MovieName = movieName });
                Session["Tickets"] = MoviePasses;
            }
            return RedirectToAction("Index");
        }
        public ActionResult ShoppingCart()
        {
            var MoviePasses = Session["Tickets"] as List<TicketViewModel>;
            if(MoviePasses == null)
            {
                return View("EmptyCart");
            }
            return View(MoviePasses);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CancelOrder()
        {
            var context = new MoviesDBMapping();
           
            var MoviePasses = Session["Tickets"] as List<TicketViewModel>;
            if (MoviePasses != null)
            {
                foreach (TicketViewModel m in MoviePasses)
                {
                    var screen = context.MoviesList.Find(m.MovieId).SeatsTaken;
                    var seat = screen.FirstOrDefault(s => s.Seat == m.SeatId);
                    screen.Remove(seat);
                    context.SaveChanges();
                }
            }
            Session["Tickets"] = null;
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Checkout()
        {
            return View("Checkout");
        }
        
        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult Checkoutpost()
        {
            string Cnumber = Request.Form["cardnumber"];
            string cvv = Request.Form["cvv"];
            string Year = Request.Form["expyear"];
            string Month = Request.Form["expmonth"];
            //credit card details validations
            if (Cnumber.Length != 16 || ! Cnumber.All(char.IsDigit) || cvv.Length != 3 || !cvv.All(char.IsDigit) || Year.Length != 4 || Month.Length != 1 ||(int.Parse(Year) < DateTime.Now.Year) || ((int.Parse(Year) == DateTime.Now.Year) && ((int.Parse(Month) < DateTime.Now.Month)))) 
            {
               
                return View("PaymentUnSuccess");
            }
            var orders = Session["Tickets"] as List<TicketViewModel>;
            if (orders != null)
            {
                var context = new MoviesDBMapping();
                Ticket ticket = new Ticket();
                foreach (TicketViewModel t in orders)
                {

                    ticket.MovieScreeningId = t.MovieId;
                    ticket.SeatNumber = t.SeatId;
                    context.Tickets.Add(ticket);
                    var Movie = context.Movies.Find(context.MoviesList.Find(t.MovieId).MovieId) ;
                    if(Movie.NumOfOrders < 0)
                    {
                        Movie.NumOfOrders = 0;
                    }
                    Movie.NumOfOrders += 1;
                    context.SaveChanges();
                    
                }
            }
            Session["Tickets"] = null;
            return View("PaymentSuccess");
        }
        public ActionResult Logout()
        {
            Session["loggedin"] = null;
            Session["userName"] = null;
            return RedirectToAction("Index");
        }
      
    



    }
}