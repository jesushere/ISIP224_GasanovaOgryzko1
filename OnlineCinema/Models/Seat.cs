using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public int HallId{ get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }

        public virtual Hall Hall { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
