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
            var viewModel = new RoleViewModel();
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new CRole
                {
                    Name = viewModel.Name,
                };

                _dbContext.CRoles.Add(role);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(viewModel);
        }
    }
}