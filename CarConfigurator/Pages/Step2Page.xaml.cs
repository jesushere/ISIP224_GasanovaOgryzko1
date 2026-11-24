using CarConfigurator.Models;
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
    public partial class Step2Page : Page
    {
        private bool isLoaded = false;
        private class PriceItem
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public override string ToString()
            {
                return Name;
            }
        }
        private List<PriceItem> colors = new List<PriceItem>
        {
            new PriceItem { Name = "White", Price = 0 },
            new PriceItem { Name = "Black", Price = 20000 },
            new PriceItem { Name = "Silver", Price = 66666 },
            new PriceItem { Name = "Red", Price = 30000 },
            new PriceItem { Name = "Blue", Price = 25000 }
        };
        public Step2Page()
        {
            InitializeComponent();

            ColorComboBox.ItemsSource = colors;
            var config = Core.Configuration;
            ColorComboBox.SelectedItem = colors.FirstOrDefault(c => c.Name == config.ColorName) ?? colors[0];
            ClimateCheckBox.IsChecked = config.SelectedOptions.Any(o => o.Name == "Climate control");
            HeatedSeatsCheckBox.IsChecked =config.SelectedOptions.Any(o => o.Name == "Heated seats");
            CameraCheckBox.IsChecked = config.SelectedOptions.Any(o => o.Name == "Rear view camera");
            LeatherCheckBox.IsChecked = config.SelectedOptions.Any(o => o.Name == "Leather interior");
            SoundCheckBox.IsChecked = config.SelectedOptions.Any(o => o.Name == "Premium audio system");
            isLoaded = true;

            UpdateColor();
            UpdateOptions();
        }
        private void ColorComboBox_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                UpdateColor();
        }
        private void UpdateColor()
        {
            if (ColorComboBox.SelectedItem == null)
                return;

            PriceItem color = (PriceItem)ColorComboBox.SelectedItem;
            Core.Configuration.ColorName = color.Name;
            Core.Configuration.ColorPrice = color.Price;
            ColorPriceText.Text = "Color surcharge: " + color.Price.ToString("N0") + " RUB";
        }
        private void Option_Checked(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
                UpdateOptions();
        }
        private void UpdateOptions()
        {
            var options = new List<CarOption>();

            if (ClimateCheckBox.IsChecked == true)
                options.Add(new CarOption { Name = "Climate control", Price = 45000 });

            if (HeatedSeatsCheckBox.IsChecked == true)
                options.Add(new CarOption { Name = "Heated seats", Price = 25000 });

            if (CameraCheckBox.IsChecked == true)
                options.Add(new CarOption { Name = "Rear view camera", Price = 30000 });

            if (LeatherCheckBox.IsChecked == true)
                options.Add(new CarOption { Name = "Leather interior", Price = 80000 });

            if (SoundCheckBox.IsChecked == true)
                options.Add(new CarOption { Name = "Premium audio system", Price = 55000 });

            Core.Configuration.SelectedOptions = options;

            OptionsPriceText.Text ="Options total: " + Core.Configuration.OptionsTotal.ToString("N0") + " RUB";
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            ((MainWindow)Application.Current.MainWindow).SetStep(1);
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            UpdateColor();
            UpdateOptions();
            NavigationService.Navigate(new Step3Page());
            ((MainWindow)Application.Current.MainWindow).SetStep(3);
        }
    }
}
