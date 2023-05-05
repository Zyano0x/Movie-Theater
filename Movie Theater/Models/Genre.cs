using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Movie_Theater.Models
{
    public class Genre
    {
        [Display(Name = "Mã Thể Loại")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên Thể Loại")]
        public string Name { get; set; }
    }
}