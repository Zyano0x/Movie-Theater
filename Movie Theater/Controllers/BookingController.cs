using Microsoft.AspNet.Identity;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using Movie_Theater.VNPay;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        //public ActionResult Create(int id)
        //{
        //    var showings = _dbContext.Showings.Where(s => s.MovieId == id && s.EndTime > DateTime.Now).ToList();
        //    var showingIds = showings.Select(s => s.Id).ToList();
        //    var theatres = _dbContext.Theatres.ToList();
        //    var theatresIds = theatres.Select(s => s.Id).ToList();

        //    var viewModel = new ShowingViewModel
        //    {
        //        Movies = _dbContext.Movies.Where(m => m.Id == id),
        //        Showings = showings,
        //        ShowingIds = showingIds,
        //        ShowingSelectId = showingIds.First(),
        //        Theatres = theatres,
        //        TheatreIds = theatresIds,
        //        TheatreSelectId = theatresIds.First()
        //    };
        //    return View(viewModel);
        //}

        public ActionResult Create(string name)
        {
            var movies = _dbContext.Movies.Where(m => m.Url == name).ToList();
            var theaters = _dbContext.Theatres.ToList();
            var defaultTheatre = theaters.FirstOrDefault(); // Get the first theater from the list

            var viewModel = new ShowingViewModel
            {
                Movies = movies,
                Theatres = theaters,
                TheatreIds = theaters.Select(s => s.Id).ToList(),
                TheatreSelectId = defaultTheatre?.Id ?? 0, // Set the selected theater ID or default to 0
            };

            if (defaultTheatre != null)
            {
                viewModel.Showings = _dbContext.Showings.Where(s => s.Movie.Url == name && s.TheatreId == defaultTheatre.Id && s.EndTime > DateTime.Now).ToList();
                viewModel.ShowingIds = _dbContext.Showings.Where(s => s.Movie.Url == name && s.TheatreId == defaultTheatre.Id && s.EndTime > DateTime.Now).Select(s => s.Id).ToList();
                viewModel.ShowingSelectId = viewModel.Showings.FirstOrDefault()?.Id ?? 0;
            }
            else
            {
                viewModel.Showings = new List<Showing>(); // Set an empty list if no theater is available
            }

            return View(viewModel);
        }

        public ActionResult GetShowingsByTheatre(int theatreId)
        {
            _dbContext.Configuration.ProxyCreationEnabled = false;
            var showings = _dbContext.Showings.Where(s => s.TheatreId == theatreId && s.EndTime > DateTime.Now).ToList();
            return Json(showings, JsonRequestBehavior.AllowGet);
        }
    }
}