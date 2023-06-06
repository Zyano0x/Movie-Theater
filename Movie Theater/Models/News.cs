using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.Models
{
    public class News
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Ảnh Bìa")]
        [Display(Name = "Hình Ảnh")]
        public string Img { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Tiêu Đề")]
        [Display(Name = "Tiêu Đề")]
        [StringLength(150)]
        public string Title { get; set; }

        public string Alias { get; set; }

        [Display(Name = "Mô Tả")]
        public string Description { get; set; }

        [AllowHtml]
        [Display(Name = "Chi Tiết")]
        public string Detail { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }

        [Display(Name = "Tác Giả")]
        public string Author { get; set; }

        [Display(Name = "Ngày Đăng")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Ngày chỉnh Sửa")]
        public DateTime ModifireDate { get; set; }
        public string Url { get; set; }

        [Display(Name = "Hiển Thị")]
        public bool IsActive { get; set; }
        [Display(Name = "Trang Chủ")]
        public bool IsHome { get; set; }
        [Display(Name = "Tin Hot")]
        public bool IsHot { get; set; }
        public bool IsFeature { get; set; }
    }
}