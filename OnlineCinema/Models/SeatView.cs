using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OnlineCinema.Models
{
    public class SeatView
    {
        public int Id { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsSelected { get; set; }
        public Brush ButtonColor
        {
            get
            {
                if (IsSelected)
                    return Brushes.LightBlue;
                return IsAvailable ? Brushes.White : Brushes.LightGray;
            }
        }
    }
}
