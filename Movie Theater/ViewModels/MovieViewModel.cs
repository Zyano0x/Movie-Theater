using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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

        [Display(Name = "Nhãn")]
        public string Rating { get; set; }

        [Display(Name = "Thời Lượng")]
        [DisplayFormat(DataFormatString = "{0:F1}m")]
        public int Runtime { get; set; }

        [Display(Name = "Doanh Thu (VNĐ)")]
        [DisplayFormat(DataFormatString = "{0:F1}M")]
        public float BoxOffice { get; set; }

        [Display(Name = "Ngôn Ngữ Chính")]
        public string OriginalLanguage { get; set; }

        [Display(Name = "Điểm Đánh Giá")]
        public int Score { get; set; }

        [Display(Name = "Nhà Phân Phối")]
        public string Distributor { get; set; }

        [Display(Name = "Trailer")]
        [DataType(DataType.Url)]
        public string TrailerUrl { get; set; }

        [Required(ErrorMessage = "vui Lòng Chọn Thể Loại")]
        public List<int> GenreIds { get; set; }
        [Display(Name = "Thể Loại")]
        public IEnumerable<Genre> Genres { get; set; }

        public List<int> CastIds { get; set; }
        [Display(Name = "Diễn Viên")]
        public IEnumerable<Crew> Casts { get; set; }

        public List<int> DirectorIds { get; set; }
        [Display(Name = "Đạo diễn")]
        public IEnumerable<Crew> Directors { get; set; }

        public List<int> ProducerIds { get; set; }
        [Display(Name = "Sản xuất")]
        public IEnumerable<Crew> Producers { get; set; }

        public List<int> WriterIds { get; set; }
        [Display(Name = "Biên kịch")]
        public IEnumerable<Crew> Writers { get; set; }

        public IEnumerable<Crew> Crews { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        public IEnumerable<MovieSchedule> MovieSchedules { get; set; }

        public virtual IEnumerable<ApplicationUser> Users { get; set; }

        [Display(Name = "Poster")]
        public string PosterPath { get; set; }
    }
}
