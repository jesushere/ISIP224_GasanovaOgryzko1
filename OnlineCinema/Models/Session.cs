using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime StartDateTime { get; set; }
        public decimal TicketPrice { get; set; }

        public virtual Movie Movie { get; set; } = null!;
        public virtual Hall Hall { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
