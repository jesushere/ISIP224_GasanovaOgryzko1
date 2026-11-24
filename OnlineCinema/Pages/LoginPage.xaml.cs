using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OnlineCinema.Models;
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
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (email == "" || password == "")
            {
                MessageBox.Show("Please enter email and password");
                return;
            }

            User? user = Core.Context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                MessageBox.Show("Incorrect email or password");
                return;
            }

            if (string.IsNullOrEmpty(user.Password))
            {
                MessageBox.Show("Invalid password");
                return;
            }

            if (!PasswordHelper.VerifyPassword(password, user.Password))
            {
                MessageBox.Show("Incorrect email or password.");
                return;
            }

            Core.CurrentUser = user;

            try
            {
                NavigationService.Navigate(new MoviesPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "smth wrong", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
        }
    }
}
