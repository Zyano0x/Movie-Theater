using Movie_Theater.Models;
using PagedList;
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

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 5;
            int pageNum = page ?? 1;
            var lstNews = from s in _dbContext.News select s;
            lstNews = lstNews.OrderByDescending(m => m.PublicationDate);
            return View(lstNews.ToPagedList(pageNum, pageSize));
        }

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