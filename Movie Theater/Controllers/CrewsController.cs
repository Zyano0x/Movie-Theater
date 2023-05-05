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
    public class CrewsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            var crew = _dbContext.Crews.ToList();
            return View(crew);
        }

        public ActionResult Details(int id)
        {
            var crew = _dbContext.Crews.Find(id);
            if (crew == null)
            {
                return HttpNotFound();
            }

            var movieC = (from m in _dbContext.MovieCrews select m).Where(m => m.CrewId == id);
            var viewModel = new CrewViewModel
            {
                Id = crew.Id,
                Name = crew.Name,
                DateOfBirth = crew.DateOfBirth,
                Birthplace = crew.Birthplace,
                Biography = crew.Biography,
                AvatarPath = crew.AvatarPath,
                Movies = (from mv in _dbContext.Movies select mv).Where(mv => movieC.Any(mc => mc.MovieId == mv.Id)),
                MovieCrews = (from mc in _dbContext.MovieCrews select mc).Where(mc => mc.CrewId == id),
                CRoles = (from cr in _dbContext.CRoles select cr)
            };

            return View(viewModel);
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
                if (Avatar != null && Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    Avatar.SaveAs(path);
                    viewModel.AvatarPath = "/Content/Images/" + fileName;
                }
                else
                {
                    viewModel.AvatarPath = "/Content/Images/Default.jpg";
                }

                var crew = new Crew
                {
                    Name = viewModel.Name,
                    DateOfBirth = viewModel.DateOfBirth,
                    Birthplace = viewModel.Birthplace,
                    Biography = viewModel.Biography,
                    AvatarPath = viewModel.AvatarPath
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

                // Save changes to the database
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(castViewModel);
        }

        public ActionResult Delete(int id)
        {
            var crew = _dbContext.Crews.Find(id);

            if (crew == null)
            {
                return HttpNotFound();
            }

            return View(crew);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var crew = _dbContext.Crews.Find(id);

            if (crew == null)
            {
                return HttpNotFound();
            }

            _dbContext.Crews.Remove(crew);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}