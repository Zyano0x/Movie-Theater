using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class Ticket
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public string Time { get; set; }

        [Column(Order = 3)]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Column(Order = 4)]
        public string MovieTitle { get; set; }

        [Column(Order = 5)]
        public DateTime TimeBooking { get; set; }

        [Column(Order = 6)]
        public int? SeatId { get; set; }
        public virtual Seat Seat { get; set; }

        [Column(Order = 7)]
        public int Status { get; set; }
    }
}