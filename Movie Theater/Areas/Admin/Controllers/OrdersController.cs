using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;

namespace Movie_Theater.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Admin/Orders
        public ActionResult Index(string Searchtext, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNum = page ?? 1;
            var booking = _dbContext.Orders
                                        .Include("Tickets")
                                        .Include("Tickets.Showing")
                                        .Include("Tickets.Showing.Movie")
                                        .ToList();
            if (!string.IsNullOrEmpty(Searchtext))
            {
                booking = booking.Where(x => x.User.UserName.ToLower().Contains(Searchtext.ToLower())).ToList();
            }
            booking = booking.OrderByDescending(m => m.OrderDate).ToList();
            return View(booking.ToPagedList(pageNum, pageSize));
        }

        [Authorize]
        [HttpPost]
        public JsonResult Cancel(int OrderId)
        {
            Order order = _dbContext.Orders.Find(OrderId);
            if (order == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }
            else
            {
                order.Status = OrderStatus.Cancelled;
                _dbContext.Entry(order).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return Json(new { success = true });
            }
        }

        public ActionResult CancelAll(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Json(new { success = false, message = "Record not found." });
            }
            else
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _dbContext.Orders.Find(Convert.ToInt32(item));
                        obj.Status = OrderStatus.Cancelled;
                        _dbContext.Entry(obj).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
        }
    }
}