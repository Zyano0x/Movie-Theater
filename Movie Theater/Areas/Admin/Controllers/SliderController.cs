using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Banner
        public ActionResult Index()
        {
            var lstSlide = from s in _dbContext.Sliders select s;
            return View(lstSlide);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider viewModel, HttpPostedFileBase Slide)
        {
            if (ModelState.IsValid)
            {
                var slide = new Slider
                {
                    Img = viewModel.Img,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                };

                _dbContext.Sliders.Add(slide);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult Edit(int Id)
        {
            var slide = _dbContext.Sliders.Find(Id);

            if (slide == null)
            {
                return HttpNotFound();
            }

            var slideR = new Slider
            {
                Img = slide.Img,
                Title = slide.Title,
                Description = slide.Description,
            };

            return View(slideR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider viewModel)
        {
            if (ModelState.IsValid)
            {
                var slide = _dbContext.Sliders.Find(viewModel.Id);

                if (slide == null)
                {
                    return HttpNotFound();
                }

                slide.Img = viewModel.Img;
                slide.Title = viewModel.Title;
                slide.Description = viewModel.Description;

                // Save changes to the database
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var slide = _dbContext.Sliders.Find(id);
            if (slide == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Sliders.Remove(slide);
            _dbContext.SaveChanges();

            return Json(new { success = true });
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