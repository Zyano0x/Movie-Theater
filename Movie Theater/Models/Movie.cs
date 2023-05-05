using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Movie_Theater.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Tên Phim")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Ngôn Ngữ Chính")]
        public string OriginalLanguage { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày Công Chiếu")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Tóm Tắt Nội Dung")]
        public string Synopsis { get; set; }

        [Required]
        [Display(Name = "Nhãn")]
        public string Rating { get; set; }

        [Display(Name = "Thời Lượng")]
        [DisplayFormat(DataFormatString = "{0:F1}m")]
        public int Runtime { get; set; }

        [Display(Name = "Doanh Thu (VNĐ)")]
        [DisplayFormat(DataFormatString = "{0:F1}M")]
        public float BoxOffice { get; set; }

        [Display(Name = "Điểm Đánh Giá")]
        public int Score { get; set; }

        [Display(Name = "Nhà Phân Phối")]
        public string Distributor { get; set; }

        [Required]
        [Display(Name = "Trailer")]
        [DataType(DataType.Url)]
        public string TrailerUrl { get; set; }

        [Display(Name = "Poster")]
        public string PosterPath { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }

        public ICollection<MovieCrew> MovieCrews { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}