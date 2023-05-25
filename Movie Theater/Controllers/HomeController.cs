using Microsoft.AspNet.Identity;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Controllers
{
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                Session["Username"] = user.Name;
            }
            var slider = _dbContext.Sliders.ToList();
            ViewBag.slider = slider;

            var movie = new MovieScheduleViewModel
            {
                Movies = _dbContext.Movies,
                MovieSchedules = _dbContext.MovieSchedules
            };
            return View(movie);
        }
	}
}