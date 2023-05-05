using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class MovieTicket
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketId { get; set; }

        [Column(Order = 2)]
        public int SchedulesId { get; set; }

        [Column(Order = 3)]
        public string UserId { get; set; }

        [Column(Order = 4)]
        public int Quantity { get; set; }

        [Column(Order = 5)]
        public int MovieId { get; set; }

        [Column(Order = 6)]
        public DateTime TimeBooking { get; set; }
    }
}