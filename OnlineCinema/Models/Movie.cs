using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Rating { get; set; }
        public DateTime StartDate { get; set; }
        public string AgeRating { get; set; } = "";
        public string PosterPath { get; set; } = "";

        public virtual ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
