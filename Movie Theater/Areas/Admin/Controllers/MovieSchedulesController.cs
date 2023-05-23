using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [Authorize(Roles = "Staff, Adminstrator")]
    public class MovieSchedulesController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: MovieSchedules
        public ActionResult Index()
        {
            var lstSchedules = _dbContext.MovieSchedules.Include("Movie").ToList();
            return View(lstSchedules);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieSchedule viewModel)
        {
            foreach (var s in _dbContext.MovieSchedules)
            {
                if (s.MovieId == viewModel.MovieId)
                {
                    if (viewModel.BeginTime <= s.EndTime && viewModel.EndTime >= s.BeginTime)
                    {
                        return this.View();
                    }
                }
            }
            var schedule = new MovieSchedule
            {
                MovieId = viewModel.MovieId,
                BeginTime = viewModel.BeginTime,
                EndTime = viewModel.EndTime,
            };
            _dbContext.MovieSchedules.Add(schedule);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var schedule = _dbContext.MovieSchedules.FirstOrDefault(s => s.MovieScheduleId == id);

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
            foreach (var s in _dbContext.MovieSchedules)
            {
                if (s.MovieId == viewModel.MovieId && s.MovieScheduleId != viewModel.MovieScheduleId)
                {
                    if (viewModel.BeginTime <= s.EndTime && viewModel.EndTime >= s.BeginTime)
                    {
                        return this.View();
                    }
                }
            }
            var schedule = _dbContext.MovieSchedules.FirstOrDefault(s => s.MovieScheduleId == viewModel.MovieScheduleId);
            schedule.BeginTime = viewModel.BeginTime;
            schedule.EndTime = viewModel.EndTime;
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