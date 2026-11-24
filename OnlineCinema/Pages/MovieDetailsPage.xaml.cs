using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
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
    public partial class MovieDetailsPage : Page
    {
        public MovieDetailsPage()
        {
            InitializeComponent();

            Movie movie = Core.SelectedMovie;

            TitleText.Text = movie.Title;
            RatingText.Text = "Rating: " + movie.Rating;
            AgeRatingText.Text = "Age rating: " + movie.AgeRating;
            DescriptionText.Text = movie.Description;
            GenresText.Text = "Genres: " + string.Join(", ", Core.Context.MovieGenres
                .Where(mg=>mg.MovieId==movie.Id)
                .Select(mg=>mg.Genre.Name)
                .ToList());

            if (!string.IsNullOrEmpty(movie.PosterPath))
            {
                PosterImage.Source = new BitmapImage(new System.Uri("pack://application:,,," + movie.PosterPath));
            }
            SessionsItemsControl.ItemsSource = Core.Context.Sessions
                .Include(s=>s.Hall)
                .Where(s=>s.MovieId==movie.Id && s.StartDateTime>System.DateTime.Now)
                .OrderBy(s=>s.StartDateTime)
                .ToList();
        }
        private void ChooseSeats_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Core.SelectedSession=(Session)button.DataContext;
            NavigationService.Navigate(new SeatSelectionPage());
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MoviesPage());
        }
    }
}
