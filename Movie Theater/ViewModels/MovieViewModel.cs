using Movie_Theater.Migrations;
using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Movie_Theater.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Tên Phim")]
        [Display(Name = "Tên Phim")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Ngày Công Chiếu")]
        [Display(Name = "Ngày Công Chiếu")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Tóm Tắt Nội Dung")]
        public string Synopsis { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Nhãn")]
        [Display(Name = "Nhãn")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Thời Lượng Phim")]
        [Display(Name = "Thời Lượng")]
        [DisplayFormat(DataFormatString = "{0:F1}m")]
        public int Runtime { get; set; }

        [Display(Name = "Điểm Đánh Giá")]
        public int Score { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Đường Dẫn Trailer")]
        [Display(Name = "Trailer")]
        [DataType(DataType.Url)]
        public string TrailerUrl { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Thể Loại")]
        public List<int> GenreIds { get; set; }
        [Display(Name = "Thể Loại")]
        public IEnumerable<Genre> Genres { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Diễn Viên")]
        public List<int> CastIds { get; set; }
        [Display(Name = "Diễn Viên")]
        public IEnumerable<Crew> Casts { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Đạo Diễn")]
        public List<int> DirectorIds { get; set; }
        [Display(Name = "Đạo diễn")]
        public IEnumerable<Crew> Directors { get; set; }

        public IEnumerable<Crew> Crews { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<MovieSchedule> MovieSchedules { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Ảnh Bìa Phim")]
        [Display(Name = "Poster")]
        public string PosterPath { get; set; }
    }
}