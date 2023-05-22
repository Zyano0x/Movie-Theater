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
            var slider = _dbContext.Slider.ToList();
            ViewBag.slider = slider;

            var movie = new ScheduleViewModel
            {
                Movies = _dbContext.Movies,
                MovieSchedules = _dbContext.MovieSchedules
            };
            return View(movie);
        }
	}
}