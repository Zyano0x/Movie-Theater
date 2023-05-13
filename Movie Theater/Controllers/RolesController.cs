using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CRole role)
        {
            if (ModelState.IsValid)
            {
                _dbContext.CRoles.Add(role);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(role);
        }
    }
}