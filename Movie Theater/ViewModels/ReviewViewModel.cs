using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm!")]
        public int Scores { get; set; }

        [Required(ErrorMessage = "Vui lòng đưa ra bình luận!")]
        public string Comment { get; set; }

        public DateTime Time { get; set; }

        public bool IsChanged { get; set; }

        public string UserName { get; set; }
    }
}