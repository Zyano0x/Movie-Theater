using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Movie_Theater.Models;
using System.Data.Entity;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Movie_Theater.Models
{
    public enum SpecialEvent { NotSpecial, Special };

    public class Showing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Mã Phim")]
        [Column(Order = 2)]
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        [Required]
        [Display(Name = "T.Gian Bắt Đầu")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:M/d/yyyy h\\:mm tt}")]
        [Column(Order = 3)]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "T.Gian Kết Thúc")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:M/d/yyyy h\\:mm tt}")]
        [Column(Order = 4)]
        public DateTime EndTime { get; set; }

        [Display(Name = "Rạp Phim")]
        [Column(Order = 5)]
        public int TheatreId { get; set; }
        public virtual Theatre Theatre { get; set; }

        [Required]
        [Display(Name = "Special Event?")]
        [Column(Order = 6)]
        public SpecialEvent SpecialEventStatus { get; set; }

        public virtual List<Ticket> Tickets { get; set; }
        public List<String> SeatList { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
        public List<int> MovieIds { get; set; }

        public IEnumerable<Theatre> Theatres { get; set; }
        public List<int> TheatreIds { get; set; }

        public Showing()
        {
            if (Tickets == null)
            {
                Tickets = new List<Ticket>();
            }
            if (SeatList == null)
            {
                SeatList = new List<String>(new String[] { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8", "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8", "C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8" });
            }
        }
    }
}