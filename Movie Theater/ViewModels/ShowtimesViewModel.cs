using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Movie_Theater.Models;

namespace Movie_Theater.ViewModels
{
    public class ShowtimesViewModel
    {
        public Movie Movie { get; set; }
        public int ShowtimesSelectId { get; set; }
        public int TheatreSelectId { get; set; }

        public List<int> ShowtimesIds { get; set; }
        public IEnumerable<Showtimes> Showtimes { get; set; }

        public List<int> TheatreIds { get; set; }
        public IEnumerable<Theatre> Theatres { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
    }
}