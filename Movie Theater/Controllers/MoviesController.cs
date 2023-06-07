using Microsoft.AspNet.Identity;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Controllers
{
    public class MoviesController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            var movie = _dbContext.Movies.ToList();
            return View(movie);
        }

        public ActionResult ShowMovies(int? page, string SearchString = "", string FilterMoviesByType = "", int FilterMovieByGenre = 0)
        {
            if (page == null) page = 1;
            //Chỉ lấy 2 loại phim : Chưa khởi chiếu && đã khởi chiếu và có lịch chiếu khả dụng
            var movies = from m in _dbContext.Movies where m.ReleaseDate > DateTime.Now || _dbContext.Showings.Any(ms => ms.MovieId == m.Id && ms.EndTime > DateTime.Now) select m;
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
                    ViewBag.name = "Phim Đang Chiếu";
                    movies = movies.Where(m => m.ReleaseDate <= DateTime.Now && _dbContext.Showings.Any(ms => ms.MovieId == m.Id && ms.EndTime > DateTime.Now));
                    ViewBag.heading = "DANH SÁCH PHIM : ĐANG CHIẾU";
                }
                else if (FilterMoviesByType == "UpcomingMovies")
                {
                    ViewBag.name = "Phim Sắp Chiếu";
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

        public ActionResult Review(int scores, string comment, int id, string userLogin, int action)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập và lưu trữ địa chỉ URL của trang chi tiết phim
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Details", "Movies", new { id = id }) });
            }
            var checkTicket = (from c in _dbContext.Tickets where c.Showing.Movie.Id == id && c.Order.User.Id == userLogin && (c.Order.Status != OrderStatus.Pending || c.Order.Status != OrderStatus.Cancelled) select c).FirstOrDefault();
            var movieUrl = _dbContext.Movies.Find(id).Url;
            if (checkTicket == null)
            {
                return RedirectToAction("Create", "Booking", new { name = movieUrl });
            }

            if (action == 1)
            {
                var review = new Review
                {
                    MovieId = id,
                    Scores = scores,
                    Comment = comment,
                    UserId = userLogin,
                    Time = DateTime.Now,
                    IsChanged = false
                };
                _dbContext.Reviews.Add(review);
            }
            else
            {
                foreach (var review in _dbContext.Reviews)
                {
                    if (review.MovieId == id && review.UserId == userLogin)
                    {
                        review.Scores = scores;
                        review.Comment = comment;
                        review.Time = DateTime.Now;
                        review.IsChanged = true;
                        break;
                    }
                }
            }

            int totalscores = scores;
            int count = 1;
            foreach (var review in _dbContext.Reviews)
            {
                if (review.MovieId == id && review.UserId != userLogin)
                {
                    totalscores += review.Scores;
                    count++;
                }
            }
            
            foreach (var movie in _dbContext.Movies)
            {
                if (movie.Id == id)
                {
                    movie.Score = Convert.ToInt32(totalscores / count);
                    break;
                }
            }

            _dbContext.SaveChanges();
            var url = _dbContext.Movies.First(x => x.Id == id).Url;
            return RedirectToAction("Details", "Movies", new { name = url });
        }

        public ActionResult Details(string name)
        {
            var userLogin = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var movieTickets = _dbContext.Tickets.ToList();
            ViewBag.CheckTicket = movieTickets.FirstOrDefault(c => c.Showing.Movie.Url == name && c.Order.User.Id == userLogin && (c.Order.Status != OrderStatus.Pending || c.Order.Status != OrderStatus.Cancelled));
            ViewBag.CheckMovieReleaseDate = (from m in _dbContext.Movies where m.ReleaseDate > DateTime.Now select m.Id).ToList();
            var movie = _dbContext.Movies
                .Include("MovieGenres.Genre")
                .Include("MovieCrews.Crew")
                .Include("Reviews")
                .FirstOrDefault(c => c.Url == name);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Synopsis = movie.Synopsis,
                Rating = movie.Rating,
                Runtime = movie.Runtime,
                Score = movie.Score,
                GenreIds = movie.MovieGenres.Select(mg => mg.Genre.Id).ToList(),
                Genres = _dbContext.Genres.ToList(),
                CastIds = (from mc in movie.MovieCrews where mc.MovieId == movie.Id && mc.CRoleId == 1 select mc.CrewId).ToList(),
                DirectorIds = (from mc in movie.MovieCrews where mc.MovieId == movie.Id && mc.CRoleId == 2 select mc.CrewId).ToList(),
                Reviews = _dbContext.Reviews.ToList(),
                Users = _dbContext.Users.ToList(),
                Crews = _dbContext.Crews.ToList(),
                Showings = _dbContext.Showings.ToList(),
                PosterPath = movie.PosterPath,
                TrailerUrl = movie.TrailerUrl,
                Url = movie.Url,
            };

            return View(viewModel);
        }
    }
}