using Movie_Theater.Models;
using Movie_Theater.Models.Common;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [Authorize(Roles = "Staff, Adminstrator")]
    public class NewsController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Banner
        public ActionResult Index()
        {
            var lstSlide = from s in _dbContext.News select s;
            return View(lstSlide);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News viewModel, HttpPostedFileBase image)
        {
            if (User.Identity.IsAuthenticated)
            {
                viewModel.Author = User.Identity.Name;
                viewModel.PublicationDate = DateTime.Today;
            }

            if (ModelState.IsValid)
            {
                var news = new News
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Content = viewModel.Content,
                    Author = viewModel.Author,
                    PublicationDate = viewModel.PublicationDate,
                    Img = viewModel.Img,
                    IsActive = true,
                    Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Title)),
                };

                _dbContext.News.Add(news);
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

            var a = new News
            {
                Img = news.Img,
                Title = news.Title,
                Content = news.Content,
                PublicationDate = news.PublicationDate,
                Description = news.Description,
            };

            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News viewModel)
        {
            if (ModelState.IsValid)
            {
                var news = _dbContext.News.Find(viewModel.Id);

                if (news == null)
                {
                    return HttpNotFound();
                }

                news.Img = viewModel.Img;
                news.Title = viewModel.Title;
                news.Description = viewModel.Description;
                news.Content = viewModel.Content;
                news.PublicationDate = viewModel.PublicationDate;
                news.Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Title));

                // Save changes to the database
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

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