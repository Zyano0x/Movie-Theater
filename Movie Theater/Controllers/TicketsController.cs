using Microsoft.AspNet.Identity;
using Microsoft.Owin.Infrastructure;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Controllers
{
    public class TicketsController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        //[Authorize]
        //public ActionResult Create(int ShowingID)
        //{
        //    Showing show = _dbContext.Showings.Find(ShowingID);

        //    Ticket tic = new Ticket();
        //    tic.Showing = show;

        //    ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
        //    Decimal TicketPrice = Models.Utilities.GenerateTicketPrice.GetTicketPrice(show.StartTime);

        //    if (show.SpecialEventStatus == SpecialEvent.NotSpecial)
        //    {
        //        // Vars for determining time between now and showing start time
        //        TimeSpan TimeBetween = (show.StartTime - DateTime.Now);

        //        // Sets discount if ticket is purchased 48 hours in advance
        //        if ((TimeBetween.TotalHours >= 48))
        //        {
        //            TicketPrice = TicketPrice - 1;
        //            ViewBag.Advance = "$1 Discount for Purchasing Early";
        //        }
        //        else
        //        {
        //            ViewBag.Advance = "No Discount";
        //        }
        //    }

        //    tic.Price = TicketPrice;

        //    ViewBag.AllSeats = SeatHelper.FindAvailableSeats(show.Tickets, User.Identity.GetUserId());
        //    return View(tic);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public ActionResult Create([Bind(Include = "Id, Seat, Price")] Ticket tic, int ShowingID, int SelectedSeat)
        //{
        //    Showing show = _dbContext.Showings.Find(ShowingID);
        //    tic.Showing = show;
        //    ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId()); //TODO: make sure this is assigned correctly

        //    // Clear existing errors - We know there is no seat
        //    ModelState.Clear();

        //    // Add the logic to see what seat they picked
        //    List<Seat> AllSeats = SeatHelper.GetAllSeats();
        //    Seat seat = AllSeats.FirstOrDefault(s => s.Id == SelectedSeat);
        //    tic.Seat = seat.Name;

        //    // Create logic that will not allow overlapping tickets 
        //    List<Ticket> BoughtTickets = _dbContext.Tickets.Where(t => t.Order.User.Id == user.Id).ToList();
        //    foreach (Ticket t in BoughtTickets)
        //    {
        //        if ((!(t.Showing.Id == ShowingID)) && (t.Showing.StartTime.Day == tic.Showing.StartTime.Day))
        //        {
        //            if (t.Showing.StartTime.Add(t.Showing.EndTime - t.Showing.StartTime) > tic.Showing.StartTime && (tic.Showing.EndTime > t.Showing.StartTime))
        //            {
        //                //ViewBag.OverlappingTicketMessage = "Error: Overlapping Movies. Select another showing.";
        //                return View("Error", new string[] { "Cannot buy overlapping Tickets!!" });
        //            }
        //        }
        //    }

        //    // Double-check everything is okay now that we've added seat
        //    ValidateModel(tic);

        //    Decimal TicketPrice = Models.Utilities.GenerateTicketPrice.GetTicketPrice(show.StartTime);

        //    if (show.SpecialEventStatus == SpecialEvent.NotSpecial)
        //    {
        //        // Vars for determining time between now and showing start time
        //        TimeSpan TimeBetween = (show.StartTime - DateTime.Now);

        //        // Sets discount if ticket is purchased 48 hours in advance
        //        if ((TimeBetween.TotalHours >= 48))
        //        {
        //            TicketPrice = TicketPrice - 1;
        //            tic.EarlyDiscount = "$1 Discount for Purchasing Early";
        //        }
        //        else
        //        {
        //            tic.EarlyDiscount = "No Discount";
        //        }
        //    }

        //    tic.Price = TicketPrice;

        //    if (ModelState.IsValid)
        //    {
        //        _dbContext.Tickets.Add(tic);
        //        _dbContext.SaveChanges();
        //        string UserId = User.Identity.GetUserId();

        //        // These six lines replace the next commented out line (Order LastOrder...)
        //        List<Order> TempOrdersList = _dbContext.Orders.Where(o => o.User.Id == UserId).ToList();
        //        Order LastOrder;
        //        if (TempOrdersList.Count() != 0)
        //            LastOrder = TempOrdersList.Last();
        //        else
        //            LastOrder = null;

        //        // Redirects the user to Orders/Create if the order is null or completed
        //        if (LastOrder == null || LastOrder.Status == OrderStatus.Complete || LastOrder.Status == OrderStatus.Cancelled)
        //        {
        //            return RedirectToAction("Create", "Orders", new { TicketID = tic.Id });
        //        }

        //        // Redirects the user to Orders/Details if the order has not been completed (they are adding more tickets to the order)
        //        else
        //        {
        //            LastOrder.Tickets.Add(tic);
        //            _dbContext.SaveChanges();
        //            return RedirectToAction("Details", "Orders", new { OrderID = LastOrder.Id });
        //        }
        //    }

        //    ViewBag.AllSeats = SeatHelper.FindAvailableSeats(tic.Showing.Tickets, User.Identity.GetUserId());
        //    return View(tic);
        //}

        [Authorize]
        public ActionResult Create(int ShowingID)
        {
            Showing show = _dbContext.Showings.Find(ShowingID);
            ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
            decimal TicketPrice = Models.Utilities.GenerateTicketPrice.GetTicketPrice(show.StartTime);

            if (show.SpecialEventStatus == SpecialEvent.NotSpecial)
            {
                // Vars for determining time between now and showing start time
                TimeSpan TimeBetween = (show.StartTime - DateTime.Now);

                // Sets discount if ticket is purchased 48 hours in advance
                if ((TimeBetween.TotalHours >= 48))
                {
                    TicketPrice = TicketPrice - 5000;
                    ViewBag.Advance = "Giảm 5K VNĐ Mua Vé Sớm";
                }
                else
                {
                    ViewBag.Advance = "Không Giảm Giá";
                }
            }

            Ticket tic = new Ticket();
            tic.Showing = show;
            tic.Price = TicketPrice;
            ViewBag.AllSeats = SeatHelper.FindAvailableSeats(show.Tickets, User.Identity.GetUserId());
            return View(tic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int ShowingID, int[] SelectedSeats)
        {
            Showing show = _dbContext.Showings.Find(ShowingID);
            ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
            decimal TicketPrice = Models.Utilities.GenerateTicketPrice.GetTicketPrice(show.StartTime);

            if (show.SpecialEventStatus == SpecialEvent.NotSpecial)
            {
                // Vars for determining time between now and showing start time
                TimeSpan TimeBetween = (show.StartTime - DateTime.Now);

                // Sets discount if ticket is purchased 48 hours in advance
                if ((TimeBetween.TotalHours >= 48))
                {
                    TicketPrice = TicketPrice - 5000;
                    ViewBag.Advance = "Giảm 5K VNĐ Mua Vé Sớm";
                }
                else
                {
                    ViewBag.Advance = "Không Giảm Giá";
                }
            }

            // Clear existing errors
            ModelState.Clear();

            // Retrieve or create the order
            Order order = _dbContext.Orders.FirstOrDefault(o => o.User.Id == user.Id && o.Status == OrderStatus.Pending);

            if (order == null)
            {
                order = new Order
                {
                    User = user,
                    Status = OrderStatus.Pending,
                    ConfirmationCode = Models.Utilities.GenerateConfirmationCode.GetNextConfirmationCode(),
                    OrderDate = DateTime.Today
                };
                _dbContext.Orders.Add(order);
            }
            else
            {
                // Clear existing tickets associated with the order
                List<Ticket> existingTickets = _dbContext.Tickets.Where(t => t.Order.Id == order.Id).ToList();
                _dbContext.Tickets.RemoveRange(existingTickets);
            }

            // Create logic that will not allow overlapping tickets 
            List<Ticket> boughtTickets = _dbContext.Tickets.Where(t => t.Order.Id == order.Id).ToList();

            foreach (int seatId in SelectedSeats)
            {
                Seat seat = SeatHelper.GetAllSeats().FirstOrDefault(s => s.Id == seatId);

                if (seat != null)
                {
                    // Create a new ticket
                    Ticket newTicket = new Ticket
                    {
                        Order = order,
                        Showing = show,
                        Seat = seat.Name,
                        Price = TicketPrice,
                        EarlyDiscount = ViewBag.Advance
                    };
                    _dbContext.Tickets.Add(newTicket);
                }
            }
            _dbContext.SaveChanges();
            ViewBag.AllSeats = SeatHelper.FindAvailableSeats(show.Tickets, User.Identity.GetUserId());
            return RedirectToAction("Details", "Orders", new { OrderID = order.Id });
        }

        [Authorize]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            Ticket ticket = _dbContext.Tickets.Find(id);

            if (ticket == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }

            Order order = ticket.Order;
            string userId = User.Identity.GetUserId();

            if (order.User.Id != userId)
            {
                return Json(new { success = false, message = "You don't have permission to delete this ticket." });
            }

            _dbContext.Tickets.Remove(ticket);
            _dbContext.SaveChanges();

            // Recalculate the subtotal, tax amount, and total
            Decimal subtotal = order.Subtotal;
            Decimal taxAmount = order.TaxAmount;
            Decimal total = order.Total;

            return Json(new { success = true, subtotal = subtotal, taxAmount = taxAmount, total = total });
        }

        //[Authorize]
        //public ActionResult Edit(int TicketID)
        //{
        //    Ticket ticket = _dbContext.Tickets.Find(TicketID);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    string userID = User.Identity.GetUserId();
        //    if (ticket.Order.User.Id == userID)
        //    {
        //        ViewBag.AllSeatsWithName = SeatHelper.FindAvailableSeatsForEdit(ticket.Showing.Tickets, userID);
        //        return View(ticket);
        //    }
        //    else
        //    {
        //        return View("Error", new string[] { "This is not your ticket!!" });
        //    }
        //}
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Seat, Price")] Ticket ticket, int[] SelectedSeats)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _dbContext.Entry(ticket).State = EntityState.Modified;
        //        _dbContext.SaveChanges();

        //        string userId = User.Identity.GetUserId();
        //        List<Order> tempOrdersList = _dbContext.Orders.Where(o => o.User.Id == userId).ToList();
        //        Order lastOrder = tempOrdersList.LastOrDefault();

        //        // Clear existing ticket selections for the order
        //        var orderTickets = _dbContext.Tickets.Where(t => t.Order.Id == lastOrder.Id).ToList();
        //        foreach (var orderTicket in orderTickets)
        //        {
        //            orderTicket.Seat = null;
        //        }

        //        // Update the selected seats
        //        if (SelectedSeats != null)
        //        {
        //            foreach (var seatId in SelectedSeats)
        //            {
        //                Seat seat = SeatHelper.GetAllSeats().FirstOrDefault(s => s.Id == seatId);
        //                if (seat != null)
        //                {
        //                    Ticket newTicket = new Ticket
        //                    {
        //                        Order = lastOrder,
        //                        Showing = ticket.Showing,
        //                        Seat = seat.Name,
        //                        Price = ticket.Price
        //                    };
        //                    _dbContext.Tickets.Add(newTicket);
        //                }
        //            }
        //        }

        //        _dbContext.SaveChanges();

        //        return RedirectToAction("Details", "Orders", new { OrderID = lastOrder.Id });
        //    }

        //    string userID = User.Identity.GetUserId();
        //    if (ticket.Order.User.Id == userID)
        //    {
        //        ViewBag.AllSeatsWithName = SeatHelper.FindAvailableSeatsForEdit(ticket.Showing.Tickets, userID);
        //    }
        //    else
        //    {
        //        return View("Error", new string[] { "This is not your ticket!!" });
        //    }

        //    return View(ticket);
        //}

        public static class SeatHelper
        {
            //public static SelectList FindAvailableSeats(List<Ticket> tickets, String UserID)
            //{
            //    //create new seat object
            //    List<Seat> TakenSeats = new List<Seat>();
            //    //create logic that will add seats already purchased to a list for filtering
            //    foreach (Ticket t in tickets)
            //    {
            //        if (t.Order != null && (t.Order.Status == OrderStatus.Complete || (t.Order.Status == OrderStatus.Pending && UserID == t.Order.User.Id)))
            //        {
            //            Seat s = new Seat();
            //            s.Name = t.Seat;
            //            s.Id = GetSeatID(s.Name);
            //            TakenSeats.Add(s);
            //        }
            //    }
            //    //filter through the seats already purchased
            //    List<Seat> AvailableSeats = GetAllSeats().Except(TakenSeats, new SeatComparer()).ToList();

            //    SelectList slAvailableSeats = new SelectList(AvailableSeats, "Id", "Name");
            //    return slAvailableSeats;
            //}

            public static List<SelectListItem> FindAvailableSeats(List<Ticket> tickets, string userID)
            {
                List<Seat> takenSeats = new List<Seat>();

                foreach (Ticket ticket in tickets)
                {
                    if (ticket.Order != null && (ticket.Order.Status == OrderStatus.Complete || (ticket.Order.Status == OrderStatus.Pending && userID == ticket.Order.User.Id)))
                    {
                        Seat seat = new Seat
                        {
                            Name = ticket.Seat,
                            Id = GetSeatID(ticket.Seat),
                            IsAvailable = false,
                        };
                        takenSeats.Add(seat);
                    }
                }

                List<Seat> allSeats = GetSeatsAvailable(takenSeats);
                List<SelectListItem> availableSeats = new List<SelectListItem>();

                foreach (Seat seat in allSeats)
                {
                    SelectListItem listItem = new SelectListItem
                    {
                        Value = seat.Id.ToString(),
                        Text = seat.Name,
                        Disabled = !seat.IsAvailable,
                    };

                    availableSeats.Add(listItem);
                }

                return availableSeats;
            }

            public class SeatComparer : IEqualityComparer<Seat>
            {
                bool IEqualityComparer<Seat>.Equals(Seat s1, Seat s2)
                {
                    return (s1.Name.Equals(s2.Name));
                }
                int IEqualityComparer<Seat>.GetHashCode(Seat obj)
                {
                    if (Object.ReferenceEquals(obj, null)) return 0;
                    return obj.Name.GetHashCode() + obj.Id;
                }
            }

            //public static SelectList FindAvailableSeatsForEdit(List<Ticket> tickets, string UserID)
            //{
            //    List<Seat> TakenSeats = new List<Seat>();

            //    foreach (Ticket t in tickets)
            //    {
            //        if (t.Order != null && (t.Order.Status == OrderStatus.Complete || (t.Order.Status == OrderStatus.Pending && UserID == t.Order.User.Id)))
            //        {
            //            Seat s = new Seat();
            //            s.Name = t.Seat;
            //            s.Id = GetSeatID(s.Name);
            //            TakenSeats.Add(s);
            //        }
            //    }

            //    List<Seat> AvailableSeats = GetAllSeats().Except(TakenSeats, new SeatComparer()).ToList();

            //    SelectList slAvailableSeats = new SelectList(AvailableSeats, "Name", "Name");
            //    return slAvailableSeats;
            //}

            //public static List<SelectListItem> FindAvailableSeatsForEdit(List<Ticket> tickets, string userID)
            //{
            //    List<Seat> takenSeats = new List<Seat>();

            //    foreach (Ticket ticket in tickets)
            //    {
            //        if (ticket.Order != null && (ticket.Order.Status == OrderStatus.Complete || (ticket.Order.Status == OrderStatus.Pending && userID == ticket.Order.User.Id)))
            //        {
            //            Seat seat = new Seat
            //            {
            //                Name = ticket.Seat,
            //                Id = GetSeatID(ticket.Seat),
            //                IsAvailable = false,
            //            };
            //            takenSeats.Add(seat);
            //        }
            //    }

            //    List<Seat> allSeats = GetSeatsAvailable(takenSeats);
            //    List<SelectListItem> availableSeats = new List<SelectListItem>();

            //    foreach (Seat seat in allSeats)
            //    {
            //        SelectListItem listItem = new SelectListItem
            //        {
            //            Value = seat.Id.ToString(),
            //            Text = seat.Name,
            //            Selected = !seat.IsAvailable,
            //            Disabled = !seat.IsAvailable,
            //        };

            //        availableSeats.Add(listItem);
            //    }

            //    return availableSeats;
            //}

            public static List<SelectListItem> FindAvailableSeatsForEdit(List<Ticket> tickets, string userID, int currentOrderId)
            {
                List<Seat> takenSeats = new List<Seat>();

                foreach (Ticket ticket in tickets)
                {
                    bool isCurrentOrder = ticket.Order != null && ticket.Order.Id == currentOrderId;

                    if (isCurrentOrder)
                    {
                        Seat seat = new Seat
                        {
                            Name = ticket.Seat,
                            Id = GetSeatID(ticket.Seat),
                            IsAvailable = false,
                            OrderId = currentOrderId,
                        };

                        takenSeats.Add(seat);
                    }
                    else if (ticket.Order != null && (ticket.Order.Status == OrderStatus.Complete || (ticket.Order.Status == OrderStatus.Pending && userID == ticket.Order.User.Id)))
                    {
                        Seat seat = new Seat
                        {
                            Name = ticket.Seat,
                            Id = GetSeatID(ticket.Seat),
                            IsAvailable = false,
                            OrderId = ticket.Order.Id,
                        };

                        takenSeats.Add(seat);
                    }
                }

                List<Seat> allSeats = GetSeatsAvailable(takenSeats);
                List<SelectListItem> availableSeats = new List<SelectListItem>();

                foreach (Seat seat in allSeats)
                {
                    bool isCurrentOrderSeat = takenSeats.Any(takenSeat => takenSeat.Name.Equals(seat.Name) && takenSeat.OrderId == currentOrderId);
                    bool isAvailable = isCurrentOrderSeat || !takenSeats.Any(takenSeat => takenSeat.Name.Equals(seat.Name));

                    SelectListItem listItem = new SelectListItem
                    {
                        Value = seat.Id.ToString(),
                        Text = seat.Name,
                        Disabled = !isAvailable,
                        Selected = isCurrentOrderSeat
                    };

                    availableSeats.Add(listItem);
                }

                return availableSeats;
            }

            public static List<Seat> GetAllSeats()
            {
                List<Seat> AllSeats = new List<Seat>();

                Seat s1 = new Seat() { Id = 1, Name = "A1", IsAvailable = true };
                AllSeats.Add(s1);

                Seat s2 = new Seat() { Id = 2, Name = "A2", IsAvailable = true };
                AllSeats.Add(s2);

                Seat s3 = new Seat() { Id = 3, Name = "A3", IsAvailable = true };
                AllSeats.Add(s3);

                Seat s4 = new Seat() { Id = 4, Name = "A4", IsAvailable = true };
                AllSeats.Add(s4);

                Seat s5 = new Seat() { Id = 5, Name = "A5", IsAvailable = true };
                AllSeats.Add(s5);

                Seat s6 = new Seat() { Id = 6, Name = "A6", IsAvailable = true };
                AllSeats.Add(s6);

                Seat s7 = new Seat() { Id = 7, Name = "A7", IsAvailable = true };
                AllSeats.Add(s7);

                Seat s8 = new Seat() { Id = 8, Name = "A8", IsAvailable = true };
                AllSeats.Add(s8);

                Seat s9 = new Seat() { Id = 9, Name = "B1", IsAvailable = true };
                AllSeats.Add(s9);

                Seat s10 = new Seat() { Id = 10, Name = "B2", IsAvailable = true };
                AllSeats.Add(s10);

                Seat s11 = new Seat() { Id = 11, Name = "B3", IsAvailable = true };
                AllSeats.Add(s11);

                Seat s12 = new Seat() { Id = 12, Name = "B4", IsAvailable = true };
                AllSeats.Add(s12);

                Seat s13 = new Seat() { Id = 13, Name = "B5", IsAvailable = true };
                AllSeats.Add(s13);

                Seat s14 = new Seat() { Id = 14, Name = "B6", IsAvailable = true };
                AllSeats.Add(s14);

                Seat s15 = new Seat() { Id = 15, Name = "B7", IsAvailable = true };
                AllSeats.Add(s15);

                Seat s16 = new Seat() { Id = 16, Name = "B8", IsAvailable = true };
                AllSeats.Add(s16);

                Seat s17 = new Seat() { Id = 17, Name = "C1", IsAvailable = true };
                AllSeats.Add(s17);

                Seat s18 = new Seat() { Id = 18, Name = "C2", IsAvailable = true };
                AllSeats.Add(s18);

                Seat s19 = new Seat() { Id = 19, Name = "C3", IsAvailable = true };
                AllSeats.Add(s19);

                Seat s20 = new Seat() { Id = 20, Name = "C4", IsAvailable = true };
                AllSeats.Add(s20);

                Seat s21 = new Seat() { Id = 21, Name = "C5", IsAvailable = true };
                AllSeats.Add(s21);

                Seat s22 = new Seat() { Id = 22, Name = "C6", IsAvailable = true };
                AllSeats.Add(s22);

                Seat s23 = new Seat() { Id = 23, Name = "C7", IsAvailable = true };
                AllSeats.Add(s23);

                Seat s24 = new Seat() { Id = 24, Name = "C8", IsAvailable = true };
                AllSeats.Add(s24);

                Seat s25 = new Seat() { Id = 25, Name = "D1", IsAvailable = true };
                AllSeats.Add(s25);

                Seat s26 = new Seat() { Id = 26, Name = "D2", IsAvailable = true };
                AllSeats.Add(s26);

                Seat s27 = new Seat() { Id = 27, Name = "D3", IsAvailable = true };
                AllSeats.Add(s27);

                Seat s28 = new Seat() { Id = 28, Name = "D4", IsAvailable = true };
                AllSeats.Add(s28);

                Seat s29 = new Seat() { Id = 29, Name = "D5", IsAvailable = true };
                AllSeats.Add(s29);

                Seat s30 = new Seat() { Id = 30, Name = "D6", IsAvailable = true };
                AllSeats.Add(s30);

                Seat s31 = new Seat() { Id = 31, Name = "D7", IsAvailable = true };
                AllSeats.Add(s31);

                Seat s32 = new Seat() { Id = 32, Name = "D8", IsAvailable = true };
                AllSeats.Add(s32);

                return AllSeats;
            }

            public static List<Seat> GetSeatsAvailable(List<Seat> takenSeats)
            {
                List<Seat> seats = GetAllSeats();

                foreach (Seat seat in seats)
                {
                    seat.IsAvailable = !takenSeats.Any(takenSeat => takenSeat.Name.Equals(seat.Name));
                }

                return seats;
            }

            public static int GetSeatID(string Name)
            {
                if (Name == "A1") return 1;
                if (Name == "A2") return 2;
                if (Name == "A3") return 3;
                if (Name == "A4") return 4;
                if (Name == "A5") return 5;
                if (Name == "A6") return 6;
                if (Name == "A7") return 7;
                if (Name == "A8") return 8;
                if (Name == "B1") return 9;
                if (Name == "B2") return 10;
                if (Name == "B3") return 11;
                if (Name == "B4") return 12;
                if (Name == "B5") return 13;
                if (Name == "B6") return 14;
                if (Name == "B7") return 15;
                if (Name == "B8") return 16;
                if (Name == "C1") return 17;
                if (Name == "C2") return 18;
                if (Name == "C3") return 19;
                if (Name == "C4") return 20;
                if (Name == "C5") return 21;
                if (Name == "C6") return 22;
                if (Name == "C7") return 23;
                if (Name == "C8") return 24;
                if (Name == "D1") return 25;
                if (Name == "D2") return 26;
                if (Name == "D3") return 27;
                if (Name == "D4") return 28;
                if (Name == "D5") return 29;
                if (Name == "D6") return 30;
                if (Name == "D7") return 31;
                if (Name == "D8") return 32;

                else return -1;
            }
        }
    }
}