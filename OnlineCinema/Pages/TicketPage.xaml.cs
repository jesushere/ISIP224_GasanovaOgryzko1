using Microsoft.EntityFrameworkCore;
using OnlineCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineCinema.Pages
{
    public partial class TicketPage : Page
    {
        public TicketPage()
        {
            InitializeComponent();

            var session = Core.Context.Sessions.First(s => s.Id == Core.SelectedSession.Id);
            session = Core.Context.Sessions
                .Where(s => s.Id == session.Id)
                .Select(s => new Session
                {
                    Id = s.Id,
                    StartDateTime = s.StartDateTime,
                    TicketPrice = s.TicketPrice,
                    Movie = Core.Context.Movies.First(m => m.Id == s.MovieId),
                    Hall = Core.Context.Halls.First(h => h.Id == s.HallId)
                })
                .First();
            var seat = Core.SelectedSeat;

            MovieText.Text = "Movie: " + session.Movie.Title;
            HallText.Text = "Hall: " + session.Hall.Name;
            DateText.Text = "Date: " + session.StartDateTime.ToString("dd.MM.yyyy HH:mm");
            SeatText.Text = "Row: " + seat.RowNumber + ", Seat: " + seat.SeatNumber;
            PriceText.Text = "Total: " + session.TicketPrice + " RUB";
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var session = Core.SelectedSession;
            var seat = Core.SelectedSeat;

            try
            {
                bool alreadyBooked = Core.Context.Tickets.Any(t =>
                    t.SessionId == session.Id &&
                    t.SeatId == seat.Id);
                if (alreadyBooked)
                {
                    MessageBox.Show("This seat has already been booked");
                    NavigationService.Navigate(new SeatSelectionPage());
                    return;
                }
                Ticket ticket = new Ticket
                {
                    UserId = Core.CurrentUser.Id,
                    SessionId = session.Id,
                    SeatId = seat.Id,
                    Price = session.TicketPrice,
                    PurchaseDate = DateTime.Now
                };

                Core.Context.Tickets.Add(ticket);
                Core.Context.SaveChanges();
                MessageBox.Show("Ticket purchased successfully!");
                Core.SelectedSeat = null;
                Core.SelectedSession = null;
                NavigationService.Navigate(new MoviesPage());
            }
            catch (DbUpdateException)
            {
                Core.Context.ChangeTracker.Clear();

                MessageBox.Show("This seat has already been booked");
                NavigationService.Navigate(new SeatSelectionPage());
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
