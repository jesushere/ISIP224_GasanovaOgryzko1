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
using OnlineCinema.Models;


namespace OnlineCinema.Pages
{
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        public void Register_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (fullName==""||email==""||password=="")
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }
            if (!email.Contains("@")||!email.Contains("."))
            {
                MessageBox.Show("Enter a valid email");
                return;
            }
            if (password.Length<6)
            {
                MessageBox.Show("Password must contain at least 6 characters");
                return;
            }
            if (Core.Context.Users.Any(u=>u.Email==email))
            {
                MessageBox.Show("This email is already registered");
                return;
            }
            User user = new User {FullName=fullName, Email=email, Password=PasswordHelper.HashPassword(password)};

            Core.Context.Users.Add(user);   
            Core.Context.SaveChanges();

            MessageBox.Show("Account created successfully!");

            NavigationService.Navigate(new LoginPage());
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }
    }
}
