using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Ảnh Slide")]
        [Display(Name = "Hình Ảnh")]
        public string Img { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Tiêu Đề")]
        [Display(Name = "Tiêu Đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Mô Tả")]
        [Display(Name = "Mô Tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Nội Dung")]
        [Display(Name = "Nội Dung")]
        public string Content { get; set; }

        [Display(Name = "Tác Giả")]
        public string Author { get; set; }

        [Display(Name = "Ngày Đăng")]
        public DateTime PublicationDate { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }
    }
}