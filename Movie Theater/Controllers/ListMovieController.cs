using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Movie_Theater.ViewModels;
using System.ComponentModel;

namespace Movie_Theater.Controllers
{
    public class ListMovieController : Controller
    {
        public readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index(int? page, string SearchString = "", string FilterMoviesByType = "", int FilterMovieByGenre = 0)
        {
            if (page == null) page = 1;
            //Chỉ lấy 2 loại phim : Chưa khởi chiếu && đã khởi chiếu và có lịch chiếu khả dụng
            var movies = from m in _dbContext.Movies where m.ReleaseDate > DateTime.Now || _dbContext.MovieSchedules.Any(ms => ms.MovieId == m.Id && ms.EndTime > DateTime.Now) select m;
            int pageSize = 10;
            int pageNum = page ?? 1;

            ////Những bộ phim chưa tới thời gian khởi chiếu.
            //var checkMovieReleaseDate = (from m in _dbContext.Movies where m.ReleaseDate > DateTime.Now select m.Id).ToList();
            ////Danh sách id các bộ phim đã khởi chiếu và có lịch chiếu khả dụng.(Chưa hết thời gian chiếu && không nằm trong danh sách ở trên)
            //var scheduleId = (from s in _dbContext.MovieSchedules where s.EndTime > DateTime.Now && (!checkMovieReleaseDate.Contains(s.MovieId)) select s.MovieId).ToList();
            var scheduleId = movies.Where(m => m.ReleaseDate <= DateTime.Now).Select(m => m.Id).ToList();
            if (scheduleId != null && scheduleId.Count > 0)
            {
                ViewBag.scheduleId = scheduleId;
            }
            else
            {
                ViewBag.scheduleId = new List<int>();
            }

            ViewBag.heading = "DANH SÁCH PHIM";
            ViewBag.name = "";

            //Tìm kiếm
            if (!String.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(m => m.Title.Contains(SearchString) || m.Synopsis.Contains(SearchString));
                ViewBag.heading = "KẾT QUẢ TÌM KIẾM '" + SearchString + "' : ";
            }
            //Lọc phim theo loại (đang chiếu , sắp chiếu ,...)
            else if (!String.IsNullOrEmpty(FilterMoviesByType))
            {
                if (FilterMoviesByType == "MoviesInTheater")
                {
                    movies = movies.Where(m => m.ReleaseDate <= DateTime.Now && _dbContext.MovieSchedules.Any(ms => ms.MovieId == m.Id && ms.EndTime > DateTime.Now));
                    ViewBag.heading = "DANH SÁCH PHIM : ĐANG CHIẾU";
                }
                else if (FilterMoviesByType == "UpcomingMovies")
                {
                    movies = movies.Where(m => m.ReleaseDate > DateTime.Now);
                    ViewBag.heading = "DANH SÁCH PHIM : SẮP CHIẾU";
                }
            }
            //Lọc phim theo thể loại (theo 1 thể loại duy nhất)
            else if (FilterMovieByGenre != 0)
            {
                var movieGenre = (from mg in _dbContext.MovieGenres select mg).Where(mg => mg.GenreId == FilterMovieByGenre);
                movies = movies.Where(m => movieGenre.Any(mg => mg.MovieId == m.Id));//chọn tất cả các phim có Id tương ứng với bất kỳ MovieId nào trong movieGenre.
                ViewBag.heading = "DANH SÁCH PHIM : " + (from g in _dbContext.Genres where g.Id == FilterMovieByGenre select g.Name).First();
                ViewBag.name = (from g in _dbContext.Genres where g.Id == FilterMovieByGenre select g.Name).First();
            }
            movies = movies.OrderBy(m => m.Title);
            return View(movies.ToPagedList(pageNum, pageSize));
        }
    }
}