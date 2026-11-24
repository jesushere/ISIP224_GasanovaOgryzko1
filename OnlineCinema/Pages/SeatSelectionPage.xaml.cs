using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OnlineCinema.Models;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineCinema.Pages
{
    public partial class SeatSelectionPage : Page
    {
        private List<SeatView> seats = new List<SeatView>();
        private bool isLoaded = false;
        public SeatSelectionPage()
        {
            InitializeComponent();

            var session = Core.SelectedSession;
            var hallSeats = Core.Context.Seats
                .Where(s=>s.HallId==session.HallId)
                .OrderBy(s=>s.RowNumber)
                .ThenBy(s=>s.SeatNumber)
                .ToList();
            var occupiedSeats = Core.Context.Tickets
                .Where(t=>t.SessionId==session.Id)
                .Select(t=>t.SeatId)
                .ToList();

            foreach(var seat in hallSeats)
            {
                seats.Add(new SeatView
                {
                    Id = seat.Id,
                    RowNumber = seat.RowNumber,
                    SeatNumber = seat.SeatNumber,
                    IsAvailable = !occupiedSeats.Contains(seat.Id)
                });
            }
            Core.SelectedSeat = null;
            isLoaded = true;
            ShowSeats();
            PriceText.Text = "Ticket price: " + session.TicketPrice + " RUB";
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            SeatView seat = (SeatView)button.DataContext;

            if (!seat.IsAvailable)
                return;
            foreach (var item in seats)
            {
                item.IsSelected = false;
            }
            seat.IsSelected = true;

            Core.SelectedSeat = Core.Context.Seats
                .First(s => s.Id == seat.Id);
            SelectedSeatText.Text = "Selected seat: Row " + seat.RowNumber + ", Seat " + seat.SeatNumber;
            SeatsItemsControl.Items.Refresh();
        }
        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (Core.SelectedSeat == null)
            {
                MessageBox.Show("Please select a seat");
                return;
            }

            NavigationService.Navigate(new TicketPage());
        }
        private void ShowSeats()
        {
            if (HideOccupiedCheckBox.IsChecked == true)
            {
                SeatsItemsControl.ItemsSource = seats.Where(s => s.IsAvailable).ToList();
            }
            else
            {
                SeatsItemsControl.ItemsSource = seats;
            }
        }
        private void HideOccupied_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                ShowSeats();
        }
        private void HideOccupied_Unchecked(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                ShowSeats();
        }
    }
}
