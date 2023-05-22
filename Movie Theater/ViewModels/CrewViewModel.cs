using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Theater.ViewModels
{
    public class CrewViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui Lòng Nhập Tên Diễn Viên")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Vui Lòng Nhập Tên Diễn Viên")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(255)]
        [Display(Name = "Nơi Sinh")]
        public string Birthplace { get; set; }

        [Display(Name = "Tiểu Sử")]
        public string Biography { get; set; }

        [Required(ErrorMessage = "Vui Lòng Chọn Ảnh Đại Diện")]
        [Display(Name = "Avatar")]
        public string AvatarPath { get; set; }

        [Display(Name = "Movies")]
        public List<int> MovieIds { get; set; }
        public IEnumerable<Movie> Movies { get; set; }

        public IEnumerable<MovieCrew> MovieCrews { get; set; }
        public IEnumerable<CRole> CRoles { get; set; }
    }
}