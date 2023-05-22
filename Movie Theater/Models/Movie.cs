using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Movie_Theater.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Display(Name = "Tên Phim")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày Công Chiếu")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Tóm Tắt Nội Dung")]
        public string Synopsis { get; set; }

        [Display(Name = "Nhãn")]
        [DisplayFormat(DataFormatString = "{0}+")]
        public int Rating { get; set; }

        [Display(Name = "T.Lượng")]
        [DisplayFormat(DataFormatString = "{0} Phút")]
        public int Runtime { get; set; }

        [Display(Name = "Điểm Đánh Giá")]
        public int Score { get; set; }

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