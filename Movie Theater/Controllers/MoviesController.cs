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
    public class MoviesController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            var movie = _dbContext.Movies.ToList();
            return View(movie);
        }

        public ActionResult Review(int scores, string comment, int id, string userLogin, int action)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập và lưu trữ địa chỉ URL của trang chi tiết phim
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Details", "Movies", new { id = id }) });
            }
            var checkTicket = (from c in _dbContext.MovieTickets where c.MovieId == id && c.UserId == userLogin select c).FirstOrDefault();
            if (checkTicket == null)
            {
                return RedirectToAction("Create", "Booking", new { id = id });
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
            ViewBag.CheckMovieReleaseDate = (from m in _dbContext.Movies where m.ReleaseDate > DateTime.Now select m.Id).ToList();
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
                MovieSchedules = _dbContext.MovieSchedules.ToList(),
                PosterPath = movie.PosterPath,
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
                if (Poster != null && Poster.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Poster.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    Poster.SaveAs(path);
                    viewModel.PosterPath = "/Content/Images/" + fileName;
                }
                else
                {
                    viewModel.PosterPath = "/Content/Images/Default.jpg";
                }

                var movie = new Movie
                {
                    Title = viewModel.Title,
                    ReleaseDate = viewModel.ReleaseDate,
                    Synopsis = viewModel.Synopsis,
                    Rating = viewModel.Rating,
                    Runtime = viewModel.Runtime,
                    TrailerUrl = viewModel.TrailerUrl,
                    PosterPath = viewModel.PosterPath,
                    MovieGenres = viewModel.GenreIds.Select(g => new MovieGenre { GenreId = g }).ToList(),
                    MovieCrews = viewModel.CastIds.Select(g => new MovieCrew { MovieId = viewModel.Id, CrewId = g, CRoleId = 1 })
                            .Concat(viewModel.DirectorIds.Select(g => new MovieCrew { MovieId = viewModel.Id, CrewId = g, CRoleId = 2 }))
                            .ToList()
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
                TrailerUrl = movie.TrailerUrl
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

            if (Poster != null && Poster.ContentLength > 0)
            {
                // Delete the old poster if it exists
                if (System.IO.File.Exists(movie.PosterPath))
                {
                    System.IO.File.Delete(movie.PosterPath);
                }

                // Save the new poster and update the movie's poster path
                var posterFileName = Path.GetFileName(Poster.FileName);
                var newPoster = Path.Combine(Server.MapPath("~/Content/Images/"), posterFileName);
                Poster.SaveAs(newPoster);
                movie.PosterPath = "/Content/Images/" + posterFileName;
            }

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

        public ActionResult Delete(int id)
        {
            var movie = _dbContext.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var movie = _dbContext.Movies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            _dbContext.Movies.Remove(movie);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}