using CarConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CarConfigurator.Pages
{
    public partial class Step5Page : Page
    {
        public Step5Page()
        {
            InitializeComponent();

            var config = Core.Configuration;
            NameTextBox.Text = config.CustomerName;
            PhoneTextBox.Text = config.Phone;
            EmailTextBox.Text = config.Email;
            ModelText.Text = "Model: " + config.ModelName;
            EngineText.Text = "Engine: " + config.EngineName;
            ColorText.Text = "Color: " + config.ColorName;

            OptionsText.Text = config.SelectedOptions.Any() ? "Options: " + string.Join(", ", config.SelectedOptions.Select(o => o.Name))
                : "Options: None";
            TotalPriceText.Text = "Total price: " + config.TotalPrice.ToString("N0") + " RUB";
            DownPaymentText.Text = "Down payment: " + config.DownPayment.ToString("N0") + " RUB";
            CreditAmountText.Text = "Credit amount: " + config.CreditAmount.ToString("N0") + " RUB";
            MonthlyPaymentText.Text = "Monthly payment: " + config.MonthlyPayment.ToString("N0") + " RUB";

            ValidateFields();
        }
        private void Contact_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateFields();
        }
        private bool ValidateFields()
        {
            string name = NameTextBox.Text.Trim();
            string phone = PhoneTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            bool validName = name.Length >= 2;
            bool validPhone = phone.Length >= 10 && phone.All(char.IsDigit);
            bool validEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            bool isValid = validName && validPhone && validEmail;
            SubmitButton.IsEnabled = isValid;
            if (isValid)
            {
                ValidationText.Text = "All fields are filled correctly";
                ValidationText.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                ValidationText.Text = "Enter a name, " + "a phone number " + "and a valid email address";
                ValidationText.Foreground = System.Windows.Media.Brushes.Red;
            }
            return isValid;
        }
        private void SaveContactData()
        {
            Core.Configuration.CustomerName = NameTextBox.Text.Trim();
            Core.Configuration.Phone = PhoneTextBox.Text.Trim();
            Core.Configuration.Email = EmailTextBox.Text.Trim();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            bool hasData =
                !string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                !string.IsNullOrWhiteSpace(PhoneTextBox.Text) ||
                !string.IsNullOrWhiteSpace(EmailTextBox.Text);
            if (hasData && !ValidateFields())
            {
                MessageBoxResult result = MessageBox.Show("Your contact information is incomplete");
                Core.Configuration.CustomerName = "";
                Core.Configuration.Phone = "";
                Core.Configuration.Email = "";
            }
            else
            {
                SaveContactData();
            }
            NavigationService.GoBack();
            ((MainWindow)Application.Current.MainWindow).SetStep(4);
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields())
                return;
            SaveContactData();
            MessageBox.Show(
                "Your application has been submitted successfully!");
            Core.Configuration = new CarConfiguration();
            NavigationService.Navigate(new Step1Page());
            ((MainWindow)Application.Current.MainWindow).SetStep(1);
        }
    }
}
