using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movie_Theater.Models;

namespace Movie_Theater.ViewModels
{
    public class ShowingViewModel
    {
        public Movie Movie { get; set; }
        public int ShowingSelectId { get; set; }
        public int TheatreSelectId { get; set; }

        public List<int> ShowingIds { get; set; }
        public IEnumerable<Showing> Showings { get; set; }

        public List<int> TheatreIds { get; set; }
        public IEnumerable<Theatre> Theatres { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
    }
}