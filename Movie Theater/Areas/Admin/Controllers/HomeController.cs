using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using System.Data.Entity;
using System.Security.Principal;
using System.Web.Configuration;
using System.Globalization;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [Authorize(Roles = "Staff, Adminstrator")]
    public class HomeController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public Boolean IsAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Adminstrator")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        String GetEmail()
        {
            var userId = User.Identity.GetUserId();
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            return user.Email;
        }

        private int GetTotalUserCountByMonth(DateTime month)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // Query the database to get the total user count for the given month
                int userCount = dbContext.Users
                                         .Where(u => u.RegistrationDate.Month == month.Month && u.RegistrationDate.Year == month.Year).Count();
                return userCount;
            }
        }

        private int GetTotalUserCountByMonth(int month)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // Query the database to get the total user count for the given month
                int userCount = dbContext.Users
                                         .Where(u => u.RegistrationDate.Month == month)
                                         .Count();
                return userCount;
            }
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                Session["Username"] = user.Name;
                Session["Email"] = GetEmail();
            }

            List<int> registrationMonths = new List<int>();
            List<int> userCounts = new List<int>();

            using (var dbContext = new ApplicationDbContext())
            {
                var query = dbContext.Users.GroupBy(u => new { Month = u.RegistrationDate.Month })
                                           .Select(g => new
                                           {
                                               RegistrationMonth = g.Key.Month,
                                               UserCount = g.Count()
                                           })
                                           .OrderBy(r => r.RegistrationMonth)
                                           .ToList();

                foreach (var result in query)
                {
                    registrationMonths.Add(result.RegistrationMonth);
                    userCounts.Add(result.UserCount);
                }
            }

            ViewBag.RegistrationMonths = registrationMonths;
            ViewBag.UserCounts = userCounts;
            ViewBag.TotalUsers = _dbContext.Users.Count();

            int previousMonth = DateTime.Now.AddMonths(-1).Month;
            int recentMonth = DateTime.Now.Month;

            int previousMonthCount = GetTotalUserCountByMonth(previousMonth);
            int recentMonthCount = GetTotalUserCountByMonth(recentMonth);

            //================================================================================
            // Calculate the percentage change
            decimal percentageChange = 0;
            if (previousMonthCount != 0)
            {
                percentageChange = ((decimal)(recentMonthCount - previousMonthCount) / previousMonthCount) * 100;
            }

            // Determine the trend (up or down)
            int trend = 0;
            if (recentMonthCount > previousMonthCount)
            {
                trend = 2; // Up
            }
            else if (recentMonthCount < previousMonthCount)
            {
                trend = 1; // Down
            }
            else
            {
                trend = 0; // No Change
            }

            // Pass the values to the view using ViewBag or a view model
            ViewBag.Percent = Math.Round(percentageChange, 1);
            ViewBag.Trend = trend;

            //================================================================================

            List<string> genreNames = new List<string>();
            List<int> movieCounts = new List<int>();

            using (var context = new ApplicationDbContext())
            {
                var genreCounts = context.MovieGenres.GroupBy(mg => mg.GenreId)
                                                     .Select(g => new
                                                     {
                                                         GenreName = g.FirstOrDefault().Genre.Name,
                                                         MovieCount = g.Count()
                                                     }).ToList();
                foreach (var genreCount in genreCounts)
                {
                    genreNames.Add(genreCount.GenreName);
                    movieCounts.Add(genreCount.MovieCount);
                }
            }

            ViewBag.GenreNames = genreNames;
            ViewBag.MovieCounts = movieCounts;
            ViewBag.TotalMovies = _dbContext.Movies.Count();
            return View();
        }
    }
}