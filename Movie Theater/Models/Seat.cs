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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Trạng thái")]
        public bool State { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá tiền!")]
        [Display(Name = "Giá")]
        public long Cost { get; set; }
    }
}