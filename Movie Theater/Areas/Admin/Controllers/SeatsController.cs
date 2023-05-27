﻿using Movie_Theater.Models;
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
            var seat = _dbContext.Seats.Include("Movie").ToList();
            return View(seat);
        }

        public ActionResult Create()
        {
            ViewBag.Movie = _dbContext.Movies.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, State, Cost, MovieId")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Seats.Add(seat);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Movie = _dbContext.Movies.ToList();
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
            ViewBag.Movie = _dbContext.Movies.ToList();
            return View(seat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, State, Cost, MovieId")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(seat).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Movie = _dbContext.Movies.ToList();
            return View(seat);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var seat = _dbContext.Seats.Find(id);
            if (seat == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            _dbContext.Seats.Remove(seat);
            _dbContext.SaveChanges();

            return Json(new { success = true });
        }

    }
}