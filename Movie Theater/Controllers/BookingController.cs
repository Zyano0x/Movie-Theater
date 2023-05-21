using Microsoft.AspNet.Identity;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using Movie_Theater.VNPay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNPAY_CS_ASPX;

namespace Movie_Theater.Controllers
{
    public class BookingController : Controller
    {
        ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int id)
        {
            var movieSchedules = _dbContext.MovieSchedules.Where(s => s.MovieId == id && s.EndTime > DateTime.Now).ToList();
            var movieScheduleIds = movieSchedules.Select(s => s.MovieScheduleId).ToList();
            var viewModel = new MovieScheduleViewModel
            {
                Movies = _dbContext.Movies.Where(m => m.Id == id),
                MovieSchedules = movieSchedules,
                MovieScheduleIds = movieScheduleIds,
                ChooseId = movieScheduleIds.First()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieScheduleViewModel viewModel, int id, int Quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập và lưu trữ địa chỉ URL của trang đặt vé phim cần đặt vé
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Create", "Booking", new { id = id, Quantity = Quantity }) });
            }
            var movieTicket = new MovieTicket
            {
                MovieId = id,
                Quantity = Quantity,
                UserId = System.Web.HttpContext.Current.User.Identity.GetUserId(),
                SchedulesId = viewModel.ChooseId,
                TimeBooking = DateTime.Now,
                Status = 0
            };
            _dbContext.MovieTickets.Add(movieTicket);
            _dbContext.SaveChanges();
            var url = Checkout(movieTicket);
            return url;
        }

        public ActionResult Checkout(MovieTicket movieTicket)
        {
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat

            var movie = _dbContext.Movies.FirstOrDefault(x => x.Id == movieTicket.MovieId);
            ViewBag.TicketId = movie.Id;
            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = movieTicket.TicketId;
            order.OrderDesc = movie.Title.ToString();
            order.Amount = 175000;
            order.Status = "0";
            order.CreatedDate = DateTime.Now;
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((order.Amount * movieTicket.Quantity) * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
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
                        var ticket = _dbContext.MovieTickets.FirstOrDefault(x => x.TicketId == orderId);
                        if (ticket != null) 
                        {
                            ticket.Status = 1;
                            _dbContext.MovieTickets.Attach(ticket);
                            _dbContext.Entry(ticket).State = System.Data.Entity.EntityState.Modified;
                            _dbContext.SaveChanges();
                        }
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn: " + orderId + " | Mã giao dịch: " + vnpayTranId;
                        
                    }
                    else
                    {
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