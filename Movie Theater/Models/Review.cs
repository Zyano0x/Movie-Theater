using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class Review
    {
        [Key]
        [Column(Order = 1)]
        public int MovieId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm!")]
        public int Scores { get; set; }
        [Required(ErrorMessage = "Vui lòng đưa ra bình luận!")]
        public string Comment { get; set; }
        public DateTime Time { get; set; }
        public bool IsChanged { get; set; }
    }
}