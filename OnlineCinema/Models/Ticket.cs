using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int  SeatId { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }

        public virtual User User { get; set; } = null; 
        public virtual Session Session { get; set; } = null!;
        public virtual Seat Seat { get; set; } = null!;
    }
}
