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
    public partial class Page3 : Page
    {
        private CarConfiguration _carConfiguration;
        public Page3(CarConfiguration carConfiguration)
        {
            InitializeComponent();
            _carConfiguration = carConfiguration;

            ModelTextBlock.Text = "Model: " + _carConfiguration.ModelName;
            EngineTextBlock.Text = "Engine: " + _carConfiguration.EngineType;
            ColorTextBlock.Text = "Color: " + _carConfiguration.Color;

            if (_carConfiguration.SelectedOptions.Count>0)
            {
                OptionsTextBlock.Text = string.Join(", ", _carConfiguration.SelectedOptions);
            }
            else
            {
                OptionsTextBlock.Text = "Options: None";
            }
            BasePriceTextBlock.Text ="Base Price: " + _carConfiguration.BasePrice.ToString("N0") + " $";
            EnginePriceTextBlock.Text ="Engine: " + _carConfiguration.EnginePrice.ToString("N0") + " $";
            ColorPriceTextBlock.Text ="Color: " + _carConfiguration.ColorPrice.ToString("N0") + " $";
            OptionsPriceTextBlock.Text ="Options: " + _carConfiguration.OptionsPrice.ToString("N0") + " $";
            TotalPriceTextBlock.Text ="Total Price: " + _carConfiguration.TotalPrice.ToString("N0") + " $";
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new Page2(_carConfiguration));
            mainWindow.SetStep(2);
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Page4(_carConfiguration));
            mainWindow.SetStep(4);
        }
    }
}
