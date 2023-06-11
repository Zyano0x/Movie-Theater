using Antlr.Runtime.Tree;
using Microsoft.AspNet.Identity;
using Movie_Theater.Models;
using Movie_Theater.Models.Common;
using Movie_Theater.ViewModels;
using Movie_Theater.VNPay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using VNPAY_CS_ASPX;
using static Movie_Theater.Controllers.TicketsController;

namespace Movie_Theater.Controllers
{
    public class OrdersController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Create(int TicketID)
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, ConfirmationCode, Complete, Subtotal, TaxAmount, Total, OrderDate")] Order order, int TicketID)
        {
            // Add first ticket to order
            Ticket ticket = _dbContext.Tickets.Find(TicketID);
            order.Tickets.Add(ticket);

            //Record date of order
            order.OrderDate = DateTime.Today;
            order.Status = OrderStatus.Pending;

            order.ConfirmationCode = Models.Utilities.GenerateConfirmationCode.GetNextConfirmationCode();

            ApplicationUser user = _dbContext.Users.Find(User.Identity.GetUserId());
            order.User = user;

            if (ModelState.IsValid)
            {
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                return RedirectToAction("Details", "Orders", new { OrderID = order.Id });
            }

            return View(order);
        }

        [Authorize]
        public ActionResult Details(int OrderID)
        {
            Order order = _dbContext.Orders.Find(OrderID);
            Session["Showtimes"] = order.Tickets.First().Showing;
            Session["Discount"] = order.Tickets.First().EarlyDiscount;
            if (order == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Admin") || User.IsInRole("Staff"))
                return View(order);
            else
            {
                if (order.User.Id == User.Identity.GetUserId())
                    return View(order);
                else
                    return View("Error", new string[] { "Oops Sorry!" });
            }

        }

        [Authorize]
        public ActionResult Edit(int OrderID)
        {
            // Retrieve the order from the database
            Order order = _dbContext.Orders.Find(OrderID);

            string userID = User.Identity.GetUserId();
            if (order.User.Id == userID)
            {
                if (!order.Tickets.Any())
                {
                    ViewBag.AvailableSeats = SeatHelper.GenerateListSeats();
                    return View(order);
                }
                else
                {
                    ViewBag.AvailableSeats = SeatHelper.FindAvailableSeatsForEdit(order.Tickets.First().Showing.Tickets, userID, OrderID);
                    return View(order);
                }
            }
            else
            {
                return HttpNotFound();
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int OrderID, int[] SelectedSeats)
        {
            // Retrieve the order from the database
            Order order = _dbContext.Orders.Include(o => o.Tickets).SingleOrDefault(o => o.Id == OrderID);

            // Ensure that the order exists
            if (order == null)
            {
                return HttpNotFound();
            }

            TicketViewModel viewModel = new TicketViewModel
            {
                Showing = order.Tickets.First().Showing,
                EarlyDiscount = order.Tickets.First().EarlyDiscount,
            };

            string UserId = User.Identity.GetUserId();

            // Check if the order belongs to the user
            if (order.User.Id != UserId)
            {
                return HttpNotFound();
            }

            // Remove existing tickets for the order
            _dbContext.Tickets.RemoveRange(order.Tickets);

            // Add new tickets for the selected seats
            foreach (var seatId in SelectedSeats)
            {
                Seat seat = SeatHelper.GetAllSeats().FirstOrDefault(s => s.Id == seatId);
                if (seat != null)
                {
                    Ticket newTicket = new Ticket
                    {
                        Order = order,
                        Showing = viewModel.Showing, // Get the showing from the first existing ticket if available
                        Seat = seat.Name,
                        Price = Models.Utilities.GenerateTicketPrice.GetTicketPrice(viewModel.Showing.StartTime),
                        EarlyDiscount = viewModel.EarlyDiscount,
                    };
                    _dbContext.Tickets.Add(newTicket);
                }
            }
            _dbContext.SaveChanges();

            ViewBag.AvailableSeats = SeatHelper.FindAvailableSeatsForEdit(order.Tickets.First().Showing.Tickets, UserId, OrderID);
            return RedirectToAction("Details", "Orders", new { OrderID = order.Id });
        }

        [Authorize]
        [HttpPost]
        public JsonResult Cancel(int OrderID)
        {
            Order order = _dbContext.Orders.Find(OrderID);
            if (order == null)
            {
                return Json(new { success = false, message = "Record not found." });
            }
            if (order.User.Id == User.Identity.GetUserId())
            {
                order.Status = OrderStatus.Cancelled;
                _dbContext.Entry(order).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "This is not your Order!" });
            }
        }

        public ActionResult Checkout(int OrderID)
        {
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat

            var OrderTicket = _dbContext.Orders.Find(OrderID);
            Session["OrderID"] = OrderID;
            Session["Order"] = OrderTicket;
            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = DateTime.Now.Ticks;
            order.OrderDesc = OrderTicket.Tickets.First().Showing.Movie.Title;
            order.Amount = (long)OrderTicket.Total;
            order.CreatedDate = DateTime.Now;
            order.Quantity = OrderTicket.Tickets.Count();
            order.Status = "0";
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((order.Amount) * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", "");
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan ve xem phim: " + order.OrderDesc);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Redirect(paymentUrl);
        }

        public ActionResult Status()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                String title = Request.QueryString["vnp_OrderInfo"];
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var order = _dbContext.Orders.Find(Convert.ToInt32(Session["OrderID"]));
                        if (order != null)
                        {
                            order.Status = OrderStatus.Complete;
                            _dbContext.Entry(order).State = System.Data.Entity.EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công " + "-" + " Mã giao dịch: " + vnpayTranId;

                        var str = "";
                        foreach (var ticket in order.Tickets)
                        {
                            str += "<tr>";
                            str += "<td style=\"font-size: 12px; font-family: 'Open Sans', sans-serif; color: #c459ee;  line-height: 18px;  vertical-align: top; padding:10px 0;\" class=\"article\">" + ticket.Showing.Movie.Title + "</td>";
                            str += "<td style=\"font-size: 12px; font-family: 'Open Sans', sans-serif; color: #646a6e; line-height: 18px; vertical-align: top; padding:10px 0;\">" + ticket.Showing.StartTime.ToString("dd/MM/yyyy HH:mm") + "</td>";
                            str += "<td style=\"font-size: 12px; font-family: 'Open Sans', sans-serif; color: #646a6e;  line-height: 18px;  vertical-align: top; padding:10px 0;\" align=\"center\">" + ticket.Seat + "</td>";
                            str += "<td style=\"font-size: 12px; font-family: 'Open Sans', sans-serif; color: #1e2b33;  line-height: 18px;  vertical-align: top; padding:10px 0;\" align=\"right\">" + Common.FormatNumber(ticket.Price, 0) + "</td>";
                            str += "</tr>";
                            str += "<tr>";
                            str += "<td height=\"1\" colspan=\"4\" style=\"border-bottom:1px solid #e4e4e4\">" + "</td>";
                            str += "</tr>";
                        }
                        string body = string.Empty;
                        using (StreamReader reader = new StreamReader(Server.MapPath("~/Views/Email/OrderConfirmation.html")))
                        {
                            body = reader.ReadToEnd();
                        }
                        body = body.Replace("{Ticket}", str);
                        body = body.Replace("{OrderID}", orderId.ToString());
                        body = body.Replace("{Username}", order.User.UserName);
                        body = body.Replace("{OrderDate}", DateTime.Now.ToString("dd/MM/yyyy"));
                        body = body.Replace("{OrderSubtotal}", Common.FormatNumber(order.Subtotal, 0));
                        body = body.Replace("{OrderTotal}", Common.FormatNumber(order.Total, 0));
                        body = body.Replace("{OrderTAX}", Common.FormatNumber(order.TaxAmount, 0));
                        Email.SendEmail(order.User.Email, "Đặt Vé Thành Công!", body, true);
                    }
                    else
                    {
                        var order = _dbContext.Orders.Find(Convert.ToInt32(Session["OrderID"]));
                        if (order != null)
                        {
                            order.Status = OrderStatus.Cancelled;
                            _dbContext.Entry(order).State = System.Data.Entity.EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn: " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                    ViewBag.Movie = title;
                    ViewBag.TmnCode = "Mã Website (Terminal ID): " + TerminalID;
                    ViewBag.TxnRef = "Mã giao dịch thanh toán: " + orderId.ToString();
                    ViewBag.VnPayTranNo = "Mã giao dịch tại VNPAY: " + vnpayTranId.ToString();
                    ViewBag.Amount = "Số tiền thanh toán (VND): " + vnp_Amount.ToString();
                    ViewBag.BankCode = "Ngân hàng thanh toán: " + bankCode;
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            return View();
        }
    }
}