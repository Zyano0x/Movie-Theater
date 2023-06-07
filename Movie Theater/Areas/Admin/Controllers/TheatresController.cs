using Movie_Theater.Models;
using PagedList;
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

        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNum = page ?? 1;
            var theatres = _dbContext.Theatres.ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                theatres = theatres.Where(x => x.Name.ToLower().Contains(Searchtext.ToLower())).ToList();
            }
            theatres = theatres.OrderBy(m => m.Id).ToList();
            return View(theatres.ToPagedList(pageNum, pageSize));
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

        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _dbContext.Theatres.Find(Convert.ToInt32(item));
                        _dbContext.Theatres.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}