using Movie_Theater.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class GenresController : Controller
    {
        ApplicationDbContext _dbContext;
        public GenresController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Genres/Index
        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNum = page ?? 1;
            var genres = from s in _dbContext.Genres select s;
            if (!string.IsNullOrEmpty(Searchtext))
            {
                genres = genres.Where(x => x.Name.Replace(" ", "").Contains(Searchtext.Replace(" ", "")));
            }
            genres = genres.OrderBy(m => m.Id);
            return View(genres.ToPagedList(pageNum, pageSize));
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

        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _dbContext.Genres.Find(Convert.ToInt32(item));
                        _dbContext.Genres.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}