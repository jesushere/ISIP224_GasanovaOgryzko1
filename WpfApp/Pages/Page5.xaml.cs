using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace WpfApp.Pages
{
    public partial class Page5 : Page
    {
        private CarConfiguration _carConfiguration;
        public Page5(CarConfiguration carConfiguration)
        {
            InitializeComponent();
            _carConfiguration = carConfiguration;

            NameTextBox.Text = _carConfiguration.ClientName;
            PhoneTextBox.Text = _carConfiguration.ClientPhone;
            EmailTextBox.Text = _carConfiguration.ClientEmail;

            SummaryTextBlock.Text = $"Model: {_carConfiguration.ModelName}\n" +
                                    $"Engine: {_carConfiguration.EngineType}\n" +
                                    $"Color: {_carConfiguration.Color}\n" +
                                    $"Options: {string.Join(", ", _carConfiguration.SelectedOptions)}\n" +
                                    $"Total Price: {_carConfiguration.TotalPrice:N0} $\n" +
                                    $"Down Payment: {_carConfiguration.DownPaymentAmount:N0} $\n" +
                                    $"Loan Months: {_carConfiguration.LoanMonths}\n" +
                                    $"Monthly Payment: {_carConfiguration.MonthlyPayment:N0} $";
            ValidateInput();
        }
        private void ValidateInput()
        {
            if (_carConfiguration == null)
            {
                return;
            }
            string name = NameTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            bool nameIsValid = name.Length >= 1;
            bool phoneIsValid = phone.Length >= 11 && phone.All(char.IsDigit);
            bool emailIsValid = email.Contains("@") && email.Contains(".");
            SubmitButton.IsEnabled = nameIsValid && phoneIsValid && emailIsValid;
            if (!nameIsValid)
            {
                ValidationTextBlock.Text = "Name cannot be empty";
            }
            else if (!phoneIsValid)
            {
                ValidationTextBlock.Text = "Phone must contain at least 11 digits";
            }
            else if (!emailIsValid)
            {
                ValidationTextBlock.Text = "Enter a valid email address";
            }
            else
            {
                ValidationTextBlock.Text = "All data is correct";
                ValidationTextBlock.Foreground = System.Windows.Media.Brushes.Green;
            }
            if (!SubmitButton.IsEnabled)
            {
                ValidationTextBlock.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_carConfiguration == null)
            {
                return;
            }

            ValidateInput();
        }
        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _carConfiguration.ClientName = NameTextBox.Text;
            _carConfiguration.ClientPhone = PhoneTextBox.Text;
            _carConfiguration.ClientEmail = EmailTextBox.Text;

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new Page4(_carConfiguration));
            mainWindow.SetStep(4);
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SubmitButton.IsEnabled)
            {
                return;
            }

            _carConfiguration.ClientName = NameTextBox.Text.Trim();
            _carConfiguration.ClientPhone = PhoneTextBox.Text.Trim();
            _carConfiguration.ClientEmail = EmailTextBox.Text.Trim();

            MessageBox.Show("Your application has been successfully submitted!", "Application", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
