using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineCinema.Pages {
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();

            var user = Core.CurrentUser;
            FullNameText.Text=user.FullName;
            EmailText.Text=user.Email;

            var tickets = Core.Context.Tickets
                .Include(t => t.Session).ThenInclude(s => s.Movie)
                .Include(t => t.Session).ThenInclude(s => s.Hall)
                .Include(t => t.Seat)
                .Where(t => t.UserId==user.Id)
                .ToList();

            TicketsListBox.ItemsSource = tickets.Select(t=>t.Session.Movie.Title+" / "+t.Session.StartDateTime.ToString("dd.MM.yyyy HH:mm")+" / "+t.Session.Hall.Name+" / Row "+t.Seat.RowNumber+" / Seat "+t.Seat.SeatNumber+" / "+t.Price+" RUB").ToList();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MoviesPage());
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;

            NavigationService.Navigate(new LoginPage());
        }
    }
}
