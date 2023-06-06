using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public enum Time { Morning, Afternoon, Evening }

    public class MoviePrice
    {
        public int id { get; set; }
        public Decimal MatineePrice { get; set; }
        public Decimal TuesdayPrice { get; set; }
        public Decimal WeekendPrice { get; set; }
        public Decimal WeekPrice { get; set; }
    }
}