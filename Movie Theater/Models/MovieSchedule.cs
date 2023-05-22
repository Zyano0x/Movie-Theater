using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class MovieSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int MovieScheduleId { get; set; }

        [Column(Order = 2)]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Column(Order = 3)]
        public DateTime BeginTime { get; set; }

        [Column(Order = 4)]
        public DateTime EndTime { get; set; }
    }
}