﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Movie_Theater.Models
{
    public class Crew
    {
        public int Id { get; set; }

        [StringLength(255)]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Đường Dẫn")]
        public string Url { get; set; }

        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(255)]
        [Display(Name = "Nơi Sinh")]
        public string Birthplace { get; set; }

        [Display(Name = "Tiểu Sử")]
        public string Biography { get; set; }

        [Display(Name = "Avatar")]
        public string AvatarPath { get; set; }

        public ICollection<MovieCrew> MovieCrews { get; set; }
    }
}
