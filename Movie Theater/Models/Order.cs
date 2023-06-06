using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Movie_Theater.Models
{
    public enum OrderStatus { Pending, Complete, Cancelled }

    public class Order
    {
        private const decimal TAX_RATE = 0.0225m;

        public int Id { get; set; }

        public int ConfirmationCode { get; set; }

        [Display(Name = "Tình Trạng")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Tổng Tiền Chưa Thuế")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal Subtotal
        {
            get
            {
                return Tickets.Sum(t => t.Price);
            }
        }

        [Display(Name = "Thuế")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal TaxAmount
        {
            get
            {
                return TAX_RATE * Subtotal;
            }
        }

        [Display(Name = "Tổng")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal Total
        {
            get
            {
                return Subtotal + TaxAmount;
            }
        }

        [Display(Name = "Ngày Đặt")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Order()
        {
            if (Tickets == null)
            {
                Tickets = new List<Ticket>();
            }
        }
    }
}