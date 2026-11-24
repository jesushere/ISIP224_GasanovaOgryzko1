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
    public partial class Step1Page : Page
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
         private List<PriceItem> models = new List<PriceItem>
         {
            new PriceItem { Name = "LADA Granta", Price = 850000 },
            new PriceItem { Name = "Haval Jolion", Price = 2099000 },
            new PriceItem { Name = "Geely Cityray", Price = 2499990 },
            new PriceItem { Name = "Tenet T8", Price = 3099000 },
            new PriceItem { Name = "Geely Monjaro", Price = 4409990 }
          };
          private List<PriceItem> engines = new List<PriceItem>
          {
            new PriceItem { Name = "Petrol", Price = 0 },
            new PriceItem { Name = "Diesel", Price = 250000 },
            new PriceItem { Name = "Electric", Price = 400000 }
          };
        public Step1Page()
        {
            InitializeComponent();

            ModelComboBox.ItemsSource = models;
            EngineComboBox.ItemsSource = engines;
            ModelComboBox.SelectedIndex = 0;
            EngineComboBox.SelectedIndex = 0;

            var config = Core.Configuration;
            if (!string.IsNullOrEmpty(config.ModelName))
            {
                ModelComboBox.SelectedItem = models.FirstOrDefault(m => m.Name == config.ModelName);
            }
            if (!string.IsNullOrEmpty(config.EngineName))
            {
                EngineComboBox.SelectedItem = engines.FirstOrDefault(e => e.Name == config.EngineName);
            }
            isLoaded = true;
            UpdateModel();
            UpdateEngine();
        }
        private void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                UpdateModel();
        }
        private void EngineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isLoaded)
                UpdateEngine();
        }
        private void UpdateModel()
        {
            if (ModelComboBox.SelectedItem == null)
                return;

            PriceItem model = (PriceItem)ModelComboBox.SelectedItem;
            Core.Configuration.ModelName = model.Name;
            Core.Configuration.BasePrice = model.Price;

            BasePriceText.Text = "Base price: " + model.Price.ToString("N0") + " RUB";
        }
        private void UpdateEngine()
        {
            if (EngineComboBox.SelectedItem == null)
                return;

            PriceItem engine = (PriceItem)EngineComboBox.SelectedItem;
            Core.Configuration.EngineName = engine.Name;
            Core.Configuration.EnginePrice = engine.Price;
            EnginePriceText.Text = "Engine surcharge: " + engine.Price.ToString("N0") + " RUB";
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (ModelComboBox.SelectedItem == null || EngineComboBox.SelectedItem == null)
            {
                MessageBox.Show("Choose a car model and engine");
                return;
            }

            NavigationService.Navigate(new Step2Page());
            ((MainWindow)Application.Current.MainWindow).SetStep(2);
        }
    }
}
