using Movie_Theater.Models;
using Movie_Theater.Models.Common;
using Movie_Theater.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class MoviesController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 1;
            int pageNum = page ?? 1;
            var movies = from s in _dbContext.Movies select s;
            if (!string.IsNullOrEmpty(Searchtext))
            {
                movies = movies.Where(x => x.Title.Contains(Searchtext));
            }
            movies = movies.OrderByDescending(m => m.ReleaseDate);
            return View(movies.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Review(int scores, string comment, int id, string userLogin, int action)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập và lưu trữ địa chỉ URL của trang chi tiết phim
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Details", "Movies", new { id = id }) });
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
            return RedirectToAction("Details", "Movies", new { id = id });
        }

        public ActionResult Details(int id)
        {
            var movie = _dbContext.Movies
                .Include("MovieGenres.Genre")
                .Include("MovieCrews.Crew")
                .Include("Reviews")
                .FirstOrDefault(m => m.Id == id);

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
                //Score = movie.Score,
                //Distributor = movie.Distributor,
                TrailerUrl = movie.TrailerUrl
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            var viewModel = new MovieViewModel
            {
                Genres = _dbContext.Genres.ToList(),
                Casts = _dbContext.Crews.ToList(),
                Directors = _dbContext.Crews.ToList(),
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieViewModel viewModel, HttpPostedFileBase Poster)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Title = viewModel.Title,
                    ReleaseDate = viewModel.ReleaseDate,
                    Synopsis = viewModel.Synopsis,
                    Rating = viewModel.Rating,
                    Runtime = viewModel.Runtime,
                    Score = 0,
                    TrailerUrl = viewModel.TrailerUrl,
                    PosterPath = viewModel.PosterPath,
                    MovieGenres = viewModel.GenreIds.Select(g => new MovieGenre { GenreId = g }).ToList(),
                    MovieCrews = viewModel.CastIds.Select(g => new MovieCrew { MovieId = viewModel.Id, CrewId = g, CRoleId = 1 })
                                        .Concat(viewModel.DirectorIds.Select(g => new MovieCrew { MovieId = viewModel.Id, CrewId = g, CRoleId = 2 }))
                                        .ToList(),
                    Url = StringHelper.ConvertText(viewModel.Title)
                };

                _dbContext.Movies.Add(movie);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Movies");
            }
            viewModel.Genres = _dbContext.Genres.ToList();
            viewModel.Casts = _dbContext.Crews.ToList();
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _dbContext.Movies
                .Include("MovieGenres.Genre")
                .Include("MovieCrews.Crew")
                .FirstOrDefault(m => m.Id == id);

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
                Casts = _dbContext.Crews.ToList(),
                DirectorIds = (from mc in movie.MovieCrews where mc.MovieId == movie.Id && mc.CRoleId == 2 select mc.CrewId).ToList(),
                Directors = _dbContext.Crews.ToList(),
                PosterPath = movie.PosterPath,
                //Score = movie.Score,
                //Distributor = movie.Distributor,
                TrailerUrl = movie.TrailerUrl,
                Url = movie.Url
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieViewModel movieViewModel, HttpPostedFileBase Poster)
        {
            if (!ModelState.IsValid)
            {
                movieViewModel.Genres = _dbContext.Genres.ToList();
                movieViewModel.Casts = _dbContext.Crews.ToList();
                movieViewModel.Directors = _dbContext.Crews.ToList();
                return View("Edit", movieViewModel);
            }

            var movie = _dbContext.Movies
                .Include("MovieGenres.Genre")
                .Include("MovieCrews.Crew")
                .Single(m => m.Id == movieViewModel.Id);

            movie.Title = movieViewModel.Title;
            movie.ReleaseDate = movieViewModel.ReleaseDate;
            movie.Synopsis = movieViewModel.Synopsis;
            movie.Rating = movieViewModel.Rating;
            movie.Runtime = movieViewModel.Runtime;
            movie.TrailerUrl = movieViewModel.TrailerUrl;
            movie.PosterPath = movieViewModel.PosterPath;
            movie.Url = movieViewModel.Url;

            // Remove existing genres that are not in the viewModel
            var genresToRemove = movie.MovieGenres.Where(mg => !movieViewModel.GenreIds.Contains(mg.GenreId)).ToList();
            foreach (var genre in genresToRemove)
            {
                movie.MovieGenres.Remove(genre);
            }

            // Add new genres from the viewModel
            var genresToAdd = movieViewModel.GenreIds.Where(gid => !movie.MovieGenres.Any(mg => mg.GenreId == gid)).ToList();
            foreach (var genreId in genresToAdd)
            {
                var movieGenre = new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = genreId
                };
                movie.MovieGenres.Add(movieGenre);
            }

            // Remove existing casts that are not in the viewModel
            var castsToRemove = movie.MovieCrews.Where(mc => !movieViewModel.CastIds.Contains(mc.CrewId) && mc.CRoleId == 1).ToList();
            foreach (var cast in castsToRemove)
            {
                movie.MovieCrews.Remove(cast);
            }

            // Add new casts from the viewModel
            var castsToAdd = movieViewModel.CastIds.Where(cid => !movie.MovieCrews.Any(mc => mc.CrewId == cid && mc.CRoleId == 1)).ToList();
            foreach (var castId in castsToAdd)
            {
                var movieCrew = new MovieCrew
                {
                    MovieId = movie.Id,
                    CrewId = castId,
                    CRoleId = 1
                };
                movie.MovieCrews.Add(movieCrew);
            }

            // Remove existing directors that are not in the viewModel
            var directorToRemove = movie.MovieCrews.Where(mc => !movieViewModel.DirectorIds.Contains(mc.CrewId) && mc.CRoleId == 2).ToList();
            foreach (var item in directorToRemove)
            {
                movie.MovieCrews.Remove(item);
            }

            // Add new directors from the viewModel
            var directorToAdd = movieViewModel.DirectorIds.Where(cid => !movie.MovieCrews.Any(mc => mc.CrewId == cid && mc.CRoleId == 2)).ToList();
            foreach (var item in directorToAdd)
            {
                var movieCast = new MovieCrew
                {
                    MovieId = movie.Id,
                    CrewId = item,
                    CRoleId = 2
                };
                movie.MovieCrews.Add(movieCast);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        //public ActionResult Delete(int id)
        //{
        //    var movie = _dbContext.Movies.Find(id);

        //    if (movie == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View(movie);
        //}
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var movie = _dbContext.Movies.Find(id);
            if (movie == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Movies.Remove(movie);
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
                        var obj = _dbContext.Movies.Find(Convert.ToInt32(item));
                        _dbContext.Movies.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Areas/Admin/Content/assets/images/PosterMovie/" + file.FileName));
            return "/Areas/Admin/Content/assets/images/PosterMovie/" + file.FileName;
        }
    }
}