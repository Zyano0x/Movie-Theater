using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Theater.Models
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        public bool State { get; set; }
        public int Cost { get; set; }
        public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
    }
}