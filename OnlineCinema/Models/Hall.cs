using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema.Models
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Rating { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
