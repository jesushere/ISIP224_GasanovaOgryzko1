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

namespace CarConfigurator.Pages
{
    public partial class Step4Page : Page
    {
        private bool isLoaded = false;

        public Step4Page()
        {
            InitializeComponent();

            var config = Core.Configuration;
            DownPaymentSlider.Value = config.DownPaymentPercent;
            LoanMonthsComboBox.SelectedIndex = (config.LoanMonths / 12) - 1;
            CarPriceText.Text = config.TotalPrice.ToString("N0") + " RUB";
            isLoaded = true;
            UpdateCredit();
        }
        private void DownPaymentSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isLoaded)
                UpdateCredit();
        }
        private void LoanMonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                UpdateCredit();
        }
        private void UpdateCredit()
        {
            if (LoanMonthsComboBox.SelectedItem == null)
                return;
            Core.Configuration.DownPaymentPercent = (int)DownPaymentSlider.Value;
            Core.Configuration.LoanMonths = (LoanMonthsComboBox.SelectedIndex + 1) * 12;
            var config = Core.Configuration;
            DownPaymentPercentText.Text = "Down payment: " + config.DownPaymentPercent + "%";
            DownPaymentText.Text = "Down payment amount: " + config.DownPayment.ToString("N0") + " RUB";
            CreditAmountText.Text = "Credit amount: " + config.CreditAmount.ToString("N0") + " RUB";
            MonthlyPaymentText.Text = "Monthly payment: " + config.MonthlyPayment.ToString("N0") + " RUB";
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            ((MainWindow)Application.Current.MainWindow).SetStep(3);
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            UpdateCredit();
            NavigationService.Navigate(new Step5Page());
            ((MainWindow)Application.Current.MainWindow).SetStep(5);
        }
    }
}
