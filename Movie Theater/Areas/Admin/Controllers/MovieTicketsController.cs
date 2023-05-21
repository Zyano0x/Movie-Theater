using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AccessDeniedAuthorize(Roles = "Adminstrator")]
    public class MovieTicketsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/MovieTickets
        public ActionResult Index()
        {
            var ticket = _dbContext.MovieTickets.ToList();
            return View(ticket);
        }
    }
}