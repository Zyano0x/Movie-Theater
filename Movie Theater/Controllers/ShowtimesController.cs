using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Controllers
{
    public class ShowtimesController : Controller
    {
        public ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Showings
        public ActionResult Index()
        {
            var showtimes = _dbContext.Showtimes.ToList();
            return View(showtimes);
        }

        public ActionResult GetShowtimesByTheater(int theatreId)
        {
            // Retrieve the showtimes for the selected theater from your data source
            var showtimes = _dbContext.Showtimes.Where(s => s.TheatreId == theatreId).ToList();

            // Generate the HTML for the showtimes dropdown
            var showtimesHtml = "<option>Chọn lịch chiếu</option>";
            foreach (var showtime in showtimes)
            {
                showtimesHtml += $"<option value='{showtime.Id}'>{showtime.StartTime.ToString("M/d/yyyy h:mm tt")}</option>";
            }

            return Content(showtimesHtml);
        }
    }
}