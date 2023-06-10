using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Movie_Theater.Models
{
    public class Ticket
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 2)]
        [Display(Name = "Giá Vé")]
        [DisplayFormat(DataFormatString = "{0:F2} VND")]
        public decimal Price { get; set; }

        [Column(Order = 3)]
        [Display(Name = "Ghế")]
        public String Seat { get; set; }

        [Column(Order = 4)]
        [Display(Name = "Giảm Giá Mua Sớm")]
        public String EarlyDiscount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Showtimes Showing { get; set; }

        public int SeatId
        {
            get
            {
                if (Seat == "A1") return 1;
                if (Seat == "A2") return 2;
                if (Seat == "A3") return 3;
                if (Seat == "A4") return 4;
                if (Seat == "A5") return 5;
                if (Seat == "A6") return 6;
                if (Seat == "A7") return 7;
                if (Seat == "A8") return 8;
                if (Seat == "B1") return 9;
                if (Seat == "B2") return 10;
                if (Seat == "B3") return 11;
                if (Seat == "B4") return 12;
                if (Seat == "B5") return 13;
                if (Seat == "B6") return 14;
                if (Seat == "B7") return 15;
                if (Seat == "B8") return 16;
                if (Seat == "C1") return 17;
                if (Seat == "C2") return 18;
                if (Seat == "C3") return 19;
                if (Seat == "C4") return 20;
                if (Seat == "C5") return 21;
                if (Seat == "C6") return 22;
                if (Seat == "C7") return 23;
                if (Seat == "C8") return 24;
                if (Seat == "D1") return 25;
                if (Seat == "D2") return 26;
                if (Seat == "D3") return 27;
                if (Seat == "D4") return 28;
                if (Seat == "D5") return 29;
                if (Seat == "D6") return 30;
                if (Seat == "D7") return 31;
                if (Seat == "D8") return 32;

                else return -1;
            }
        }
    }
}