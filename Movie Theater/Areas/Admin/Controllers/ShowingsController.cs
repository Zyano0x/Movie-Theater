using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class ShowingsController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            var showtimes = _dbContext.Showings.Include("Movie").ToList();
            return View(showtimes);
        }

        public ActionResult Create(string str = "", int choose = 1)
        {
            ViewBag.Error = str;
            var viewModel = new Showing
            {
                //Khi load lại trang trường chọn phim đã chọn vẫn còn
                MovieId = choose,
                MovieIds = _dbContext.Movies.Select(item => item.Id).ToList(),
                Movies = _dbContext.Movies,

                TheatreId = choose,
                TheatreIds = _dbContext.Theatres.Select(item => item.Id).ToList(),
                Theatres = _dbContext.Theatres,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Showing viewModel)
        {
            var movie = (from m in _dbContext.Movies where m.Id == viewModel.MovieId select m).First();
            if (viewModel.StartTime < DateTime.Now.AddDays(1))
            {
                return RedirectToAction("Create", new { str = "Không để trống & thời gian bắt đầu phải cách thời gian thêm lịch 24H", choose = viewModel.MovieId });
            }

            foreach (var s in _dbContext.Showings)
            {
                if (s.MovieId == viewModel.MovieId && (viewModel.StartTime <= s.EndTime && viewModel.StartTime.AddMinutes(movie.Runtime) >= s.StartTime))
                {
                    return RedirectToAction("Create", new { str = "Lịch chiếu bị trùng!", choose = viewModel.MovieId });
                }
            }

            var schedule = new Showing
            {
                MovieId = viewModel.MovieId,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.StartTime.AddMinutes(movie.Runtime),
                TheatreId = viewModel.TheatreId,
            };
            _dbContext.Showings.Add(schedule);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string str = "")
        {
            ViewBag.Error = str;
            var schedule = _dbContext.Showings.FirstOrDefault(s => s.Id == id);
            schedule.MovieIds = _dbContext.Movies.Select(m => m.Id).ToList();
            schedule.Movies = _dbContext.Movies;
            schedule.TheatreIds = _dbContext.Theatres.Select(m => m.Id).ToList();
            schedule.Theatres = _dbContext.Theatres;

            if (schedule == null)
            {
                return HttpNotFound();
            }

            return View(schedule);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Showing viewModel)
        {
            var movie = (from m in _dbContext.Movies where m.Id == viewModel.MovieId select m).First();
            var schedule = _dbContext.Showings.FirstOrDefault(s => s.Id == viewModel.Id);
            if (viewModel.StartTime < schedule.StartTime)
            {
                return RedirectToAction("Edit", new { str = "Không để trống & lịch chiếu mới > lịch chiếu cũ", choose = viewModel.MovieId });
            }
            foreach (var s in _dbContext.Showings)
            {
                if (s.MovieId == viewModel.MovieId && s.Id != viewModel.Id)
                {
                    if (viewModel.StartTime <= s.EndTime && viewModel.StartTime.AddMinutes(movie.Runtime) >= s.StartTime)
                    {
                        return RedirectToAction("Edit", new { str = "Lịch chiếu bị trùng!" });
                    }
                }
            }

            schedule.MovieId = viewModel.MovieId;
            schedule.StartTime = viewModel.StartTime;
            schedule.EndTime = viewModel.StartTime.AddMinutes(movie.Runtime);
            schedule.TheatreId = viewModel.TheatreId;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var schedule = _dbContext.Showings.Find(id);
            if (schedule == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Showings.Remove(schedule);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }
    }
}