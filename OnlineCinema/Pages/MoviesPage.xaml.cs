using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OnlineCinema.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineCinema.Pages
{
    public partial class MoviesPage : Page
    {
        private bool isInitialized = false;
        private List<Movie> allMovies = new List<Movie>();
        public MoviesPage()
        {
            InitializeComponent();
            allMovies = Core.Context.Movies.ToList();
            isInitialized = true;
            ShowMovies();
        }
        private void UserTextBlockSetup()
        {
        }
        private void ShowMovies()
        {
            string search = SearchTextBox.Text.ToLower();
            var movies = allMovies.Where(m=>m.Title.ToLower().Contains(search));
            if (SortComboBox.SelectedIndex==1)
            {
                movies = movies.OrderByDescending(m=>m.Rating);
            }
            else if (SortComboBox.SelectedIndex==2)
            {
                movies = movies.OrderBy(m=>m.Rating);
            }
            else
            {
                movies = movies.OrderBy(m=>m.Title);
            }
            MoviesItemsControl.ItemsSource = movies.ToList();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isInitialized)
                ShowMovies();
        }
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (isInitialized)
                ShowMovies();
        }
        private void Movie_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Core.SelectedMovie = (Movie)button.DataContext;
            NavigationService.Navigate(new MovieDetailsPage());
        }
        private void Account_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AccountPage());
        }
    }
}
