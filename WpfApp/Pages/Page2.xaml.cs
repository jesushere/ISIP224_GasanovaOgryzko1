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

namespace WpfApp.Pages
{
    public partial class Page2 : Page
    {
        private CarConfiguration _carConfiguration;
        public Page2(CarConfiguration carConfiguration)
        {
            InitializeComponent();
            _carConfiguration = carConfiguration;

            if (_carConfiguration.Color != null)
            {
                foreach (ComboBoxItem item in ColorComboBox.Items)
                {
                    if (item.Content.ToString() == _carConfiguration.Color)
                    {
                        ColorComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
            NightVisionCheckBox.IsChecked = _carConfiguration.SelectedOptions.Contains("Night Vision");
            StarlightHeadliinerCheckBox.IsChecked = _carConfiguration.SelectedOptions.Contains("Starlight Headliner");
            ElectrochromicRoofCheckBox.IsChecked = _carConfiguration.SelectedOptions.Contains("Electrochromic Roof");
            RearCameraCheckBox.IsChecked = _carConfiguration.SelectedOptions.Contains("Rear Camera");
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedColor = ColorComboBox.SelectedItem as ComboBoxItem;

            string colorName = selectedColor.Content.ToString();

            decimal colorPrice = 0;

            switch (colorName)
            {
                case "White":
                    colorPrice = 0;
                    break;
                case "Gray":
                    colorPrice = 4500;
                    break;
                case "Gold":
                    colorPrice = 22000;
                    break;
                case "Black":
                    colorPrice = 0;
                    break;
                case "Red":
                    colorPrice = 0;
                    break;
            }

            _carConfiguration.SelectedOptions.Clear();
            decimal optionsPrice = 0;

            if (NightVisionCheckBox.IsChecked == true)
            {
                _carConfiguration.SelectedOptions.Add("Night Vision");
                optionsPrice += 3500;
            }
            if (StarlightHeadliinerCheckBox.IsChecked == true)
            {
                _carConfiguration.SelectedOptions.Add("Starlight Headliner");
                optionsPrice += 15000;
            }
            if (ElectrochromicRoofCheckBox.IsChecked == true)
            {
                _carConfiguration.SelectedOptions.Add("Electrochromic Roof");
                optionsPrice += 4500;
            }
            if (RearCameraCheckBox.IsChecked == true)
            {
                _carConfiguration.SelectedOptions.Add("Rear Camera");
                optionsPrice += 800;
            }

            _carConfiguration.Color = colorName;
            _carConfiguration.ColorPrice = colorPrice;
            _carConfiguration.OptionsPrice = optionsPrice;

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new Page3(_carConfiguration));
            mainWindow.SetStep(3);
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Page1(_carConfiguration));
            mainWindow.SetStep(1);
        }
    }
}
