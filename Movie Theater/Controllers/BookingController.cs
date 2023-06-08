using Microsoft.AspNet.Identity;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using Movie_Theater.VNPay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using VNPAY_CS_ASPX;

namespace Movie_Theater.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult History()
        {
            var userId = User.Identity.GetUserId();
            var bookingHistory = _dbContext.Orders
                                        .Include("Tickets")
                                        .Include("Tickets.Showing")
                                        .Include("Tickets.Showing.Movie")
                                        .Where(x => x.User.Id == userId)
                                        .ToList();
            return View(bookingHistory);
        }

        //public ActionResult Create(string name)
        //{
        //    var movies = _dbContext.Movies.Where(m => m.Url == name).ToList();
        //    var theaters = _dbContext.Theatres.ToList();
        //    var defaultTheatre = theaters.FirstOrDefault(); // Get the first theater from the list

        //    var viewModel = new ShowingViewModel
        //    {
        //        Movies = movies,
        //        Theatres = theaters,
        //        TheatreIds = theaters.Select(s => s.Id).ToList(),
        //        TheatreSelectId = defaultTheatre?.Id ?? 0, // Set the selected theater ID or default to 0
        //    };

        //    if (defaultTheatre != null)
        //    {
        //        viewModel.Showings = _dbContext.Showings.Where(s => s.Movie.Url == name && s.TheatreId == defaultTheatre.Id && s.EndTime > DateTime.Now).ToList();
        //        viewModel.ShowingIds = _dbContext.Showings.Where(s => s.Movie.Url == name && s.TheatreId == defaultTheatre.Id && s.EndTime > DateTime.Now).Select(s => s.Id).ToList();
        //        viewModel.ShowingSelectId = viewModel.Showings.FirstOrDefault()?.Id ?? 0;
        //    }
        //    else
        //    {
        //        viewModel.Showings = new List<Showing>(); // Set an empty list if no theater is available
        //    }

        //    return View(viewModel);
        //}

        public ActionResult Create(string name)
        {
            var movie = _dbContext.Movies.FirstOrDefault(m => m.Url == name);
            var movies = _dbContext.Movies.Where(m => m.Url == name).ToList();
            var theaters = _dbContext.Theatres.ToList();
            var defaultTheatre = theaters.FirstOrDefault();
            if (movie == null)
            {
                // Handle the case when the movie is not found
                // You can return an error view or redirect to an error page
                return RedirectToAction("NotFound", "Error");
            }
            var viewModel = new ShowtimesViewModel
            {
                Movie = movie,
                Movies = movies,
                Theatres = theaters,
                TheatreIds = theaters.Select(s => s.Id).ToList(),
                TheatreSelectId = defaultTheatre?.Id ?? 0,
            };

            if (defaultTheatre != null)
            {
                // Filter showings by movie and default theater
                viewModel.Showtimes = _dbContext.Showtimes.Where(s => s.Movie.Id == movie.Id && s.TheatreId == defaultTheatre.Id && s.EndTime > DateTime.Now).ToList();
                viewModel.ShowtimesIds = _dbContext.Showtimes.Where(s => s.Movie.Url == name && s.TheatreId == defaultTheatre.Id && s.EndTime > DateTime.Now).Select(s => s.Id).ToList();
                viewModel.ShowtimesSelectId = viewModel.Showtimes.FirstOrDefault()?.Id ?? 0;
            }
            else
            {
                viewModel.Showtimes = new List<Showtimes>();
            }

            return View(viewModel);
        }

        public ActionResult GetShowings(string name, int theatreId)
        {
            _dbContext.Configuration.ProxyCreationEnabled = false;
            var showings = _dbContext.Showtimes.Where(s => s.Movie.Url == name && s.TheatreId == theatreId && s.EndTime > DateTime.Now).ToList();

            return Json(showings, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetShowingsByTheatre(int theatreId)
        //{
        //    _dbContext.Configuration.ProxyCreationEnabled = false;
        //    var showings = _dbContext.Showings.Where(s => s.TheatreId == theatreId && s.EndTime > DateTime.Now).ToList();
        //    return Json(showings, JsonRequestBehavior.AllowGet);
        //}

        [Authorize]
        [HttpPost]
        public JsonResult Cancel(int OrderId)
        {
            Order order = _dbContext.Orders.Find(OrderId);
            if (order == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }
            if (order.User.Id == User.Identity.GetUserId())
            {
                order.Status = OrderStatus.Cancelled;
                _dbContext.Entry(order).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "This is not your Order!" });
            }
        }
    }
}