﻿using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Areas.Admin.Controllers
{
    [AccessDeniedAuthorize(Roles = "Staff, Adminstrator")]
    public class MovieTicketsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/MovieTickets
        public ActionResult Index()
        {
            var ticket = _dbContext.MovieTickets.Include("Movie").Include("MovieSchedule").Include("User").ToList();
            return View(ticket);
        }

        public ActionResult Edit(int id)
        {
            var ticket = _dbContext.MovieTickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieTicket ticket)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(ticket).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }
    }
}