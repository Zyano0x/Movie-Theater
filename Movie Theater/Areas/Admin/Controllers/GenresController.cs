using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [Authorize(Roles = "Staff, Adminstrator")]
    public class GenresController : Controller
    {
        ApplicationDbContext _dbContext;
        public GenresController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Genres/Index
        public ActionResult Index()
        {
            var genre = _dbContext.Genres.ToList();
            return View(genre);
        }

        // GET: Genres/Details/ID
        public ActionResult Details(int id)
        {
            var genre = _dbContext.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Genres.Add(genre);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        // GET: Genres/Edit/Id
        public ActionResult Edit(int id)
        {
            var genre = _dbContext.Genres.Find(id);
            if (genre == null)
            {
                return HttpNotFound();
            }
            return View(genre);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(genre).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genre);
        }

        // GET: Genres/Delete/Id
        //public ActionResult Delete(int id)
        //{
        //    var genre = _dbContext.Genres.Find(id);
        //    if (genre == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(genre);
        //}
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var genre = _dbContext.Genres.Find(id);
            if (genre == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Genres.Remove(genre);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }
    }
}