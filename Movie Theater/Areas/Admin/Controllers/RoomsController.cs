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
    [AdminAuthorize(Roles = "Staff, Adminstrator")]
    public class RoomsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Admin/Room
        public ActionResult Index()
        {
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
                };

                _dbContext.Rooms.Add(room);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            return this.View(viewModel);
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