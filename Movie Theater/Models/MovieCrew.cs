using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class MovieCrew
    {
        [Key]
        [Column(Order = 1)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CrewId { get; set; }
        public Crew Crew { get; set; }

        [Key]
        [Column(Order = 3)]
        public int CRoleId { get; set; }
        public CRole CRole { get; set; }
    }
}