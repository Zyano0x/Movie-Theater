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

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Email = GetEmail();
                ViewBag.Username = user.Name;
            }
            return View();
        }
    }
}