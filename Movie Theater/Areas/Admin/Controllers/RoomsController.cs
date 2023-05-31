using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    public class RoomsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Admin/Room
        public ActionResult Index()
        {
            ViewData["ListSeat"] = _dbContext.RoomSeats.ToList();
            var room = _dbContext.Rooms.ToList();
            return View(room);
        }

        public ActionResult Create()
        {
            var viewModel = new RoomViewModel
            {
                Seats = _dbContext.Seats.ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoomViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var room = new Room
                {
                    State = viewModel.State,
                    RoomSeat = viewModel.SeatIds.Select(g => new RoomSeat { RoomId = viewModel.Id, SeatId = g, Seat_State = true, Seat_State_Book = false }).ToList()
                };

                _dbContext.Rooms.Add(room);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return this.View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var room = _dbContext.Rooms
                .Include("RoomSeat.Seat")
                .FirstOrDefault(m => m.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            var viewModel = new RoomViewModel
            {
                State = room.State,
                SeatIds = (from r in room.RoomSeat where r.RoomId == room.Id && r.Seat_State == true select r.SeatId).ToList(),
                Seats = _dbContext.Seats.ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoomViewModel roomViewModel)
        {
            if (!ModelState.IsValid)
            {
                roomViewModel.Seats = _dbContext.Seats.ToList();
                return View("Edit", roomViewModel);
            }

            var room = _dbContext.Rooms
                .Include("RoomSeat.Seat")
                .Single(m => m.Id == roomViewModel.Id);

            room.State = roomViewModel.State;

            // Remove existing casts that are not in the viewModel
            room.RoomSeat.Where(mc => !roomViewModel.SeatIds.Contains(mc.Seat.Id))
            .ToList()
            .ForEach(seat => seat.Seat_State = false);

            room.RoomSeat.Where(mc => roomViewModel.SeatIds.Contains(mc.Seat.Id))
            .ToList()
            .ForEach(seat => seat.Seat_State = true);


            // Add new casts from the viewModel
            var seatsToAdd = roomViewModel.SeatIds.Where(s => !room.RoomSeat.Any(rs => rs.SeatId == s && rs.RoomId == roomViewModel.Id)).ToList();
            foreach (var item in seatsToAdd)
            {
                var seat = new RoomSeat
                {
                    RoomId = roomViewModel.Id,
                    SeatId = item,
                    Seat_State = true,
                    Seat_State_Book = false,
                };
                room.RoomSeat.Add(seat);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public JsonResult Disable(int id)
        {
            var room = _dbContext.Rooms.Find(id);
            if (room == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            room.State = false;
            _dbContext.Rooms.Attach(room);
            _dbContext.Entry(room).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult Enable(int id)
        {
            var room = _dbContext.Rooms.Find(id);
            if (room == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            room.State = true;
            _dbContext.Rooms.Attach(room);
            _dbContext.Entry(room).State = System.Data.Entity.EntityState.Modified;
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }
    }
}