using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Movie_Theater.Models.Common;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [Authorize(Roles = "Staff, Adminstrator")]
    public class CrewsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            var crew = _dbContext.Crews.ToList();
            return View(crew);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrewViewModel viewModel, HttpPostedFileBase Avatar)
        {
            if (ModelState.IsValid)
            {
                var crew = new Crew
                {
                    Name = viewModel.Name,
                    DateOfBirth = viewModel.DateOfBirth,
                    Birthplace = viewModel.Birthplace,
                    Biography = viewModel.Biography,
                    AvatarPath = viewModel.AvatarPath,
                    Url = StringHelper.ConvertText(StringHelper.RemoveDiacritics(viewModel.Name))
                };

                _dbContext.Crews.Add(crew);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var crew = _dbContext.Crews.Find(id);

            if (crew == null)
            {
                return HttpNotFound();
            }

            var castViewModel = new CrewViewModel
            {
                Id = crew.Id,
                Name = crew.Name,
                DateOfBirth = crew.DateOfBirth,
                Birthplace = crew.Birthplace,
                Biography = crew.Biography,
                AvatarPath = crew.AvatarPath,
            };

            return View(castViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CrewViewModel castViewModel)
        {
            if (ModelState.IsValid)
            {
                var crew = _dbContext.Crews.Find(castViewModel.Id);

                if (crew == null)
                {
                    return HttpNotFound();
                }

                crew.Name = castViewModel.Name;
                crew.DateOfBirth = castViewModel.DateOfBirth;
                crew.Birthplace = castViewModel.Birthplace;
                crew.Biography = castViewModel.Biography;
                crew.AvatarPath = castViewModel.AvatarPath;

                // Save changes to the database
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(castViewModel);
        }

        //public ActionResult Delete(int id)
        //{
        //    var crew = _dbContext.Crews.Find(id);

        //    if (crew == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(crew);
        //}
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var crew = _dbContext.Crews.Find(id);
            if (crew == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Crews.Remove(crew);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Areas/Admin/Content/assets/images/CrewAvatar/" + file.FileName));
            return "/Areas/Admin/Content/assets/images/CrewAvatar/" + file.FileName;
        }
    }
}