using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    public class TheatresController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            var model = _dbContext.Theatres.ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Theatre theatre)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Theatres.Add(theatre);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(theatre);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var theatre = _dbContext.Theatres.Find(id);
            if (theatre == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Theatres.Remove(theatre);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }
    }
}