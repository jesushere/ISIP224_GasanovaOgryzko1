using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCinema
{
    public class Core
    {
        public static CinemaContext Context = new CinemaContext();
        public static User? CurrentUser;
        public static Movie SelectedMovie;
        public static Session SelectedSession;
        public static Seat SelectedSeat;
    }
}
