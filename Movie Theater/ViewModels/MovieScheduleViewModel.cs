using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movie_Theater.Models;

namespace Movie_Theater.ViewModels
{
    public class ScheduleViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<MovieSchedule> MovieSchedules { get; set; }
        public List<int> MovieScheduleIds { get; set; }
        public int ChooseId { get; set; }
    }
}