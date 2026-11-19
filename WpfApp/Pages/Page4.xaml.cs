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
    public partial class Page4 : Page
    {
        private CarConfiguration _carConfiguration;
        public Page4(CarConfiguration carConfiguration)
        {
            InitializeComponent();
            _carConfiguration = carConfiguration;

            DownPaymentSlider.Value = _carConfiguration.DownPayment;
            int index = (_carConfiguration.LoanMonths/12)-1;
            LoanMonthsComboBox.SelectedIndex = index;
            UpdateCalculation();
        }
        private void UpdateCalculation()
        {
            if (_carConfiguration == null)
            {
                return;
            }
            _carConfiguration.DownPayment = (int)DownPaymentSlider.Value;
            ComboBoxItem selectedItem = (ComboBoxItem)LoanMonthsComboBox.SelectedItem;
            if (selectedItem != null)
            {
                string monthsText = selectedItem.Content.ToString();
                _carConfiguration.LoanMonths = int.Parse(monthsText.Split(' ')[0]);
            }
            DownPaymentPercentTextBlock.Text = $"Down payment: {_carConfiguration.DownPayment}%";
            TotalPriceTextBlock.Text = $"Total price: {_carConfiguration.TotalPrice}";
            DownPaymentTextBlock.Text = $"Down payment amount: {_carConfiguration.DownPaymentAmount}";
            CreditAmountTextBlock.Text = $"Credit amount: {_carConfiguration.CreditAmount}";
            MonthlyPaymentTextBlock.Text = $"Monthly payment: {_carConfiguration.MonthlyPayment}";
        }
        private void DownPaymentSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_carConfiguration != null)
            {
                UpdateCalculation();
            }
        }
        private void LoanMonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_carConfiguration != null && LoanMonthsComboBox.SelectedItem != null)
            {
                UpdateCalculation();
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Page3(_carConfiguration));
            mainWindow.SetStep(3);
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Page5(_carConfiguration));
            mainWindow.SetStep(5);
        }
    }
}
