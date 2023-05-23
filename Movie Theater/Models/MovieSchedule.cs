using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Movie_Theater.Models;
using System.Data.Entity;

namespace Movie_Theater.Models
{
    public class MovieSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int MovieScheduleId { get; set; }

        [Required]
        [Display(Name = "Mã Phim")]
        [Column(Order = 2)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Required]
        [Display(Name = "Tên Phim")]
        [Column(Order = 3)]
        public string MovieTitle { get; set; }

        [Required]
        [Display(Name = "T.Gian Bắt Đầu")]
        [Column(Order = 4)]
        public DateTime BeginTime { get; set; }

        [Required]
        [Display(Name = "T.Gian Kết Thúc")]
        [Column(Order = 5)]
        public DateTime EndTime { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
        public List<int> MovieIds { get; set; }
    }
}