using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Movie_Theater.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Trạng thái")]
        public bool State { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Ghế")]
        public List<int> SeatIds { get; set; }
        [Display(Name = "Danh sách ghế")]
        public IEnumerable<Seat> Seats { get; set; }
    }
}