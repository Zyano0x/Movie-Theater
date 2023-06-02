using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Controllers
{
    public class NewsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: News
        public ActionResult Details(string name)
        {
            var newsArticle = _dbContext.News.FirstOrDefault(n => n.Url == name);
            if (newsArticle == null)
            {
                return HttpNotFound();
            }

            ViewBag.NewsArticle = newsArticle.Title; 
            return View(newsArticle);
        }
    }
}