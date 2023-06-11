using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Movie_Theater.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int MovieId { get; set; }
        [JsonIgnore]
        public Movie Movie { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm!")]
        public int Scores { get; set; }

        [Required(ErrorMessage = "Vui lòng đưa ra bình luận!")]
        public string Comment { get; set; }

        public DateTime Time { get; set; }

        public bool IsChanged { get; set; }
    }
}