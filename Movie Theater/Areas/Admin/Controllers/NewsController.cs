using Movie_Theater.Models;
using Movie_Theater.Models.Common;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;


namespace Movie_Theater.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class NewsController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Banner
        public ActionResult Index(string Searchtext,int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNum = page ?? 1;
            var lstNews = from s in _dbContext.News select s;
            if(!string.IsNullOrEmpty(Searchtext))
            {
                lstNews = lstNews.Where(x => x.Title.Replace(" ", "").Contains(Searchtext.Replace(" ", "")));
            }
            lstNews = lstNews.OrderByDescending(m => m.PublicationDate);
            return View(lstNews.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Title));
                viewModel.ModificationDate = DateTime.Now;
                viewModel.Author = User.Identity.Name;
                viewModel.PublicationDate = DateTime.Now;
                viewModel.IsActive = true;
                _dbContext.News.Add(viewModel);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult Edit(int Id)
        {
            var news = _dbContext.News.Find(Id);

            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Title));
                viewModel.ModificationDate = DateTime.Now;
                viewModel.Author = User.Identity.Name;
                _dbContext.News.Attach(viewModel);
                _dbContext.Entry(viewModel).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(News viewModel, HttpPostedFileBase image)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        viewModel.Author = User.Identity.Name;
        //        viewModel.PublicationDate = DateTime.Today;
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var news = new News
        //        {
        //            Title = viewModel.Title,
        //            Description = viewModel.Description,
        //            Detail = viewModel.Detail,
        //            Author = viewModel.Author,
        //            PublicationDate = viewModel.PublicationDate,
        //            Img = viewModel.Img,
        //            IsActive = true,
        //            Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Title)),
        //        };

        //        _dbContext.News.Add(news);
        //        _dbContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(viewModel);
        //}

        //public ActionResult Edit(int Id)
        //{
        //    var news = _dbContext.News.Find(Id);

        //    if (news == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var a = new News
        //    {
        //        Img = news.Img,
        //        Title = news.Title,
        //        Detail = news.Detail,
        //        PublicationDate = news.PublicationDate,
        //        Description = news.Description,
        //    };

        //    return View(news);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(News viewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var news = _dbContext.News.Find(viewModel.Id);

        //        if (news == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        news.Img = viewModel.Img;
        //        news.Title = viewModel.Title;
        //        news.Description = viewModel.Description;
        //        news.Detail = viewModel.Detail;
        //        news.PublicationDate = viewModel.PublicationDate;
        //        news.Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Title));

        //        // Save changes to the database
        //        _dbContext.SaveChanges();

        //        return RedirectToAction("Index");
        //    }
        //    return View(viewModel);
        //}

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var slide = _dbContext.News.Find(id);
            if (slide == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.News.Remove(slide);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult IsActive(int id)
        {
            var item = _dbContext.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true, isAcive = item.IsActive });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult IsHome(int id)
        {
            var item = _dbContext.News.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                _dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true, isHome = item.IsHome });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult IsHot(int id)
        {
            var item = _dbContext.News.Find(id);
            if (item != null)
            {
                item.IsHot = !item.IsHot;
                _dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true, isHot = item.IsHot });
            }

            return Json(new { success = false });
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
                        var obj = _dbContext.News.Find(Convert.ToInt32(item));
                        _dbContext.News.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Areas/Admin/Content/assets/images/Slider/" + file.FileName));
            return "/Areas/Admin/Content/assets/images/Slider/" + file.FileName;
        }
    }
}