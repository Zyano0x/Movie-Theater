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
            var newsHome = _dbContext.News.Where(n => n.IsActive == true && n.IsHome == true).ToList();
            var newsHot = _dbContext.News.Where(n => n.IsActive == true && n.IsHot == true && n.IsHome == false).ToList();
            ViewBag.NewsHome = newsHome;
            ViewBag.NewsHot = newsHot;

            var movie = new ShowingViewModel
            {
                Movies = _dbContext.Movies,
                Showings = _dbContext.Showings
            };
            return View(movie);
        }
	}
}