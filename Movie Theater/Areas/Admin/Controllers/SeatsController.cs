using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    public class SeatsController : Controller
    {
        ApplicationDbContext _dbContext;
        public SeatsController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Admin/Seats
        public ActionResult Index()
        {
            var seat = _dbContext.Seats.ToList();
            return View(seat);
        }

        public ActionResult Create()
        {
            //ViewBag.Movie = _dbContext.Movies.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Seat seat, int count_chairs = 1)//[Bind(Include = "Id, State, Cost, MovieId")]
        {
            var this_seat = new Seat()
            {
                Cost = seat.Cost,
                State = seat.State,
            };

            if (ModelState.IsValid)
            {
                for (int i = 0; i < count_chairs; i++)
                {
                    _dbContext.Seats.Add(this_seat);
                    _dbContext.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            //ViewBag.Movie = _dbContext.Movies.ToList();
            return View(seat);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Seat seat = _dbContext.Seats.Find(id);
            if (seat == null)
            {
                return HttpNotFound();
            }

            //ViewBag.Movie = _dbContext.Movies.ToList();
            return View(seat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Seat seat)// [Bind(Include = "Id, State, Cost, MovieId")] 
        {
            if (ModelState.IsValid)
            {
                var lstSeat = _dbContext.RoomSeats.Where(s => s.SeatId == seat.Id).ToList();
                foreach (var item in lstSeat)
                {
                    item.Seat_State = seat.State;
                }

                _dbContext.Entry(seat).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Movie = _dbContext.Movies.ToList();
            return View(seat);
        }

        [HttpPost]
        public JsonResult Disable(int id)
        {
            var seat = _dbContext.Seats.Find(id);
            if (seat == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            var lstSeat = _dbContext.RoomSeats.Where(s => s.SeatId == seat.Id).ToList();
            foreach (var item in lstSeat)
            {
                item.Seat_State = false;
            }

            seat.State = false;
            _dbContext.Seats.Attach(seat);
            _dbContext.Entry(seat).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Enable(int id)
        {
            var seat = _dbContext.Seats.Find(id);
            if (seat == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            var lstSeat = _dbContext.RoomSeats.Where(s => s.SeatId == seat.Id).ToList();
            foreach (var item in lstSeat)
            {
                item.Seat_State = true;
            }

            seat.State = true;
            _dbContext.Seats.Attach(seat);
            _dbContext.Entry(seat).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

        //[HttpPost]
        //public JsonResult Delete(int id)
        //{
        //    var seat = _dbContext.Seats.Find(id);
        //    if (seat == null)
        //    {
        //        return Json(new { success = false, message = "Record not found." });
        //    }

        //    _dbContext.Seats.Remove(seat);
        //    _dbContext.SaveChanges();

        //    return Json(new { success = true });
        //}

    }
}