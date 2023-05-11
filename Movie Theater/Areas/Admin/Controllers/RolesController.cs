using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Roles
        public ActionResult Index()
        {
            var role = _dbContext.CRoles.ToList();
            return View(role);
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

        public ActionResult Edit(int id)
        {
            var role = _dbContext.CRoles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CRole role)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(role).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var role = _dbContext.CRoles.Find(id);
            if (role == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.CRoles.Remove(role);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }
    }
}