using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Movie_Theater.Models
{
    public class CRole
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Vai Trò")]
        public string Name { get; set; }
    }
}