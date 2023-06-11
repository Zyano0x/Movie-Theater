using Movie_Theater.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class ShowtimesController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index(string Searchtext, int? page)
        {
            //Chỉ hiển thị những suất chiếu còn hạn.
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNum = page ?? 1;
            var showtimes = _dbContext.Showtimes.Include("Movie").Where(item => item.EndTime > DateTime.Now).ToList();
            if (!string.IsNullOrEmpty(Searchtext) && showtimes != null)
            {
                showtimes = showtimes.Where(x => x.Movie.Title.ToLower().Contains(Searchtext.ToLower())).ToList();
            }
            showtimes = showtimes.OrderBy(m => m.StartTime).ToList();
            return View(showtimes.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Create(string str = "", int choose = 1)
        {
            ViewBag.Error = str;
            var viewModel = new Showtimes
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
        public ActionResult Create(Showtimes viewModel)
        {
            var movie = (from m in _dbContext.Movies where m.Id == viewModel.MovieId select m).First();
            if (viewModel.StartTime < DateTime.Now.AddDays(1))
            {
                return RedirectToAction("Create", new { str = "Không để trống & thời gian bắt đầu phải cách thời gian thêm lịch 24H", choose = viewModel.MovieId });
            }

            foreach (var s in _dbContext.Showtimes)
            {
                if (s.MovieId == viewModel.MovieId && s.TheatreId == viewModel.TheatreId && (viewModel.StartTime <= s.EndTime && viewModel.StartTime.AddMinutes(movie.Runtime) >= s.StartTime))
                {
                    return RedirectToAction("Create", new { str = "Lịch chiếu bị trùng!", choose = viewModel.MovieId });
                }
            }

            var schedule = new Showtimes
            {
                MovieId = viewModel.MovieId,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.StartTime.AddMinutes(movie.Runtime),
                TheatreId = viewModel.TheatreId,
            };
            _dbContext.Showtimes.Add(schedule);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id, string str = "")
        {
            ViewBag.Error = str;
            var schedule = _dbContext.Showtimes.FirstOrDefault(s => s.Id == id);
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
        public ActionResult Edit(Showtimes viewModel)
        {
            var movie = (from m in _dbContext.Movies where m.Id == viewModel.MovieId select m).First();
            var schedule = _dbContext.Showtimes.FirstOrDefault(s => s.Id == viewModel.Id);
            if (viewModel.StartTime < schedule.StartTime)
            {
                return RedirectToAction("Edit", new { str = "Không để trống & lịch chiếu mới > lịch chiếu cũ", choose = viewModel.MovieId });
            }
            foreach (var s in _dbContext.Showtimes)
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
            var schedule = _dbContext.Showtimes.Find(id);
            if (schedule == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Showtimes.Remove(schedule);
            _dbContext.SaveChanges();

            return Json(new { success = true });
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
                        var obj = _dbContext.Showtimes.Find(Convert.ToInt32(item));
                        _dbContext.Showtimes.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}