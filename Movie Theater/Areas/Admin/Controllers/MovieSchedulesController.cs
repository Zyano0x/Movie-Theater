using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class MovieSchedulesController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: MovieSchedules
        public ActionResult Index()
        {
            var lstSchedules = _dbContext.MovieSchedules.Include("Movie").ToList();
            return View(lstSchedules);
        }

        public ActionResult Create(string str = "", int choose = 1)
        {
            ViewBag.Error = str;
            var viewModel = new MovieSchedule
            {
                //Khi load lại trang trường chọn phim đã chọn vẫn còn
                MovieId = choose,
                MovieIds = _dbContext.Movies.Select(item => item.Id).ToList(),
                Movies = _dbContext.Movies,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieSchedule viewModel)
        {
            var movie = (from m in _dbContext.Movies where m.Id == viewModel.MovieId select m).First();
            if (viewModel.BeginTime < DateTime.Now.AddDays(1))
            {
                return RedirectToAction("Create", new { str = "Không để trống & thời gian bắt đầu phải cách thời gian thêm lịch 24H", choose = viewModel.MovieId });
            }

            foreach (var s in _dbContext.MovieSchedules)
            {
                if (s.MovieId == viewModel.MovieId && (viewModel.BeginTime <= s.EndTime && viewModel.BeginTime.AddMinutes(movie.Runtime) >= s.BeginTime))
                {
                    return RedirectToAction("Create", new { str = "Lịch chiếu bị trùng!", choose = viewModel.MovieId });
                }
            }

            var schedule = new MovieSchedule
            {
                MovieId = viewModel.MovieId,
                BeginTime = viewModel.BeginTime,
                EndTime = viewModel.BeginTime.AddMinutes(movie.Runtime),
            };
            _dbContext.MovieSchedules.Add(schedule);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string str = "")
        {
            ViewBag.Error = str;
            var schedule = _dbContext.MovieSchedules.FirstOrDefault(s => s.Id == id);
            schedule.MovieIds = _dbContext.Movies.Select(m => m.Id).ToList();
            schedule.Movies = _dbContext.Movies;

            if (schedule == null)
            {
                return HttpNotFound();
            }

            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieSchedule viewModel)
        {
            var movie = (from m in _dbContext.Movies where m.Id == viewModel.MovieId select m).First();
            var schedule = _dbContext.MovieSchedules.FirstOrDefault(s => s.Id == viewModel.Id);
            if (viewModel.BeginTime < schedule.BeginTime)
            {
                return RedirectToAction("Edit", new { str = "Không để trống & lịch chiếu mới > lịch chiếu cũ", choose = viewModel.MovieId });
            }
            foreach (var s in _dbContext.MovieSchedules)
            {
                if (s.MovieId == viewModel.MovieId && s.Id != viewModel.Id)
                {
                    if (viewModel.BeginTime <= s.EndTime && viewModel.BeginTime.AddMinutes(movie.Runtime) >= s.BeginTime)
                    {
                        return RedirectToAction("Edit", new { str = "Lịch chiếu bị trùng!" });
                    }
                }
            }

            schedule.MovieId = viewModel.MovieId;
            schedule.BeginTime = viewModel.BeginTime;
            schedule.EndTime = viewModel.BeginTime.AddMinutes(movie.Runtime);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var schedule = _dbContext.MovieSchedules.Find(id);
            if (schedule == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.MovieSchedules.Remove(schedule);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }
    }
}