using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Movie_Theater.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Giá Vé")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public Decimal Price { get; set; }

        [Display(Name = "Ghế")]
        public String Seat { get; set; }

        [Display(Name = "Giảm Giá Mua Sớm")]
        public String EarlyDiscount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Showtimes Showing { get; set; }
    }
}