using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Theater.Models
{
    public class MovieTicket
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [Column(Order = 2)]
        public int MovieScheduleId { get; set; }
        public MovieSchedule MovieSchedule { get; set; }

        [Column(Order = 3)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Column(Order = 4)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Column(Order = 5)]
        public DateTime TimeBooking { get; set; }

        [Column(Order = 6)]
        public int Status { get; set; }
    }
}