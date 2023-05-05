using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Movie_Theater.ViewModels
{
    public class CrewViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Ngày Sinh")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(255)]
        [Display(Name = "Nơi sinh")]
        public string Birthplace { get; set; }

        [Display(Name = "Tiểu Sử")]
        public string Biography { get; set; }

        [Display(Name = "Avatar")]
        public string AvatarPath { get; set; }

        [Display(Name = "Movies")]
        public List<int> MovieIds { get; set; }
        public IEnumerable<Movie> Movies { get; set; }

        public IEnumerable<MovieCrew> MovieCrews { get; set; }
        public IEnumerable<CRole> CRoles { get; set; }
    }
}