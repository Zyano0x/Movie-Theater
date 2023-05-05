using Movie_Theater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Theater.ViewModels
{
    public class MovieScheduleViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<MovieSchedule> MovieSchedules { get; set; }
        public List<int> MovieScheduleIds { get; set; }
        public int ChooseId { get; set; }
    }
}