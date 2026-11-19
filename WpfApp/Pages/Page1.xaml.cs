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
    public partial class Page1 : Page
    {
        private CarConfiguration _configuration;
        public Page1(CarConfiguration configuration)
        {
            InitializeComponent();

            ModelComboBox.SelectionChanged += ModelComboBox_SelectionChanged;

            UpdateCarImage(); //для фоток тема

            _configuration = configuration;
        }
        private void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCarImage();
        }

        private void UpdateCarImage()
        {
            ComboBoxItem selectedItem = ModelComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem == null || CarImage == null)
                return;

            string model = selectedItem.Content.ToString();

            switch (model)
            {
                case "Lamborghini Aventador SVJ":CarImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/aventador.jpg", UriKind.Relative));
                    break;

                case "Ferrari SF90 Stradale":CarImage.Source = new System.Windows.Media.Imaging.BitmapImage(
                        new Uri("/Images/sf90.jpg", UriKind.Relative));
                    break;

                case "McLaren 765LT":CarImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/765lt.jpg", UriKind.Relative));
                    break;

                case "Porsche 911 GT3 RS":CarImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/gt3rs.jpg", UriKind.Relative));
                    break;

                case "Aston Martin DBS Superleggera":CarImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Images/dbs.jpg", UriKind.Relative));
                    break;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedModel = ModelComboBox.SelectedItem as ComboBoxItem;

            string modelName = selectedModel?.Content.ToString();

            decimal basePrice = 0;

            switch(modelName)
            {
                case "BMW M4":
                    basePrice = 5700000;
                    break;
                case "Porsche 911 GT3 RS":
                    basePrice = 39970000;
                    break;
                case "Toyota Supra IV":
                    basePrice = 12000000;
                    break;
            }

            string engineType = "";
            decimal enginePrice = 0;

            if (PetrolRadioButton.IsChecked == true)
            {
                engineType = "Petrol";
                enginePrice = 0;
            }
            else if (DieselRadioButton.IsChecked == true)
            {
                engineType = "Diesel";
                enginePrice = 250000;
            }
            else if (ElectricRadioButton.IsChecked == true)
            {
                engineType = "Electrical";
                enginePrice = 580000;
            }

            _configuration.ModelName = modelName;
            _configuration.EngineType = engineType;
            _configuration.EnginePrice = enginePrice;
            _configuration.BasePrice = basePrice;

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.MainFrame.Navigate(new Page2(_configuration));
            mainWindow.SetStep(2);
        }
    }
}
