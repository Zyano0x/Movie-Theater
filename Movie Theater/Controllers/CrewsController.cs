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

        public ActionResult Details(string name)
        {
            var crew = _dbContext.Crews.FirstOrDefault(c => c.Url == name);
            if (crew == null)
            {
                return HttpNotFound();
            }

            var movieC = (from m in _dbContext.MovieCrews select m).Where(m => m.Crew.Url == name);
            var viewModel = new CrewViewModel
            {
                Id = crew.Id,
                Name = crew.Name,
                DateOfBirth = crew.DateOfBirth,
                Birthplace = crew.Birthplace,
                Biography = crew.Biography,
                AvatarPath = crew.AvatarPath,
                Movies = (from mv in _dbContext.Movies select mv).Where(mv => movieC.Any(mc => mc.MovieId == mv.Id)),
                MovieCrews = (from mc in _dbContext.MovieCrews select mc).Where(mc => mc.Crew.Url == name),
                CRoles = (from cr in _dbContext.CRoles select cr)
            };
            ViewBag.Name = viewModel.Name;
            return View(viewModel);
        }
    }
}