using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Controllers
{
    public class SliderController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Banner
        public ActionResult Index()
        {
            var lstSlide = from s in _dbContext.Slider select s;
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

                _dbContext.Slider.Add(slide);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult Edit(int Id)
        {
            var slide = _dbContext.Slider.Find(Id);

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
                var slide = _dbContext.Slider.Find(viewModel.Id);

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
        public ActionResult Delete(int id)
        {
            var slide = _dbContext.Slider.Find(id);

            if (slide == null)
            {
                return HttpNotFound();
            }

            return View(slide);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var slide = _dbContext.Slider.Find(id);

            if (slide == null)
            {
                return HttpNotFound();
            }

            _dbContext.Slider.Remove(slide);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/Images/Slider/" + file.FileName));
            return "/Content/Images/Slider/" + file.FileName;
        }
    }
}