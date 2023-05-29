using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class RoomSeat
    {
        [Key]
        [Column(Order = 1)]
        public int RoomId { get; set; }
        public Room Room { get; set; }

        [Key]
        [Column(Order = 2)]
        public int SeatId { get; set; }
        public Seat Seat { get; set; }

        [Column(Order = 3)]
        public bool Seat_State { get; set; }
        
        [Column(Order = 4)]
       public bool Seat_State_Book { get; set; }
    }
}