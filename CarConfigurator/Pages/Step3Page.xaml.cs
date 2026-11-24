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
    public partial class Step3Page : Page
    {
        public Step3Page()
        {
            InitializeComponent();

            var config = Core.Configuration;
            ModelText.Text = "Model: " + config.ModelName;
            EngineText.Text = "Engine: " + config.EngineName;
            ColorText.Text = "Color: " + config.ColorName;

            if (config.SelectedOptions.Any())
            {
                OptionsText.Text = string.Join("\n", config.SelectedOptions.Select(o => o.Name + " - " + o.Price.ToString("N0") + " RUB"));
            }
            else
            {
                OptionsText.Text = "No additional options selected";
            }
            BasePriceText.Text = "Base price: " + config.BasePrice.ToString("N0") + " RUB";

            EnginePriceText.Text = "Engine surcharge: " + config.EnginePrice.ToString("N0") + " RUB";

            ColorPriceText.Text = "Color surcharge: " + config.ColorPrice.ToString("N0") + " RUB";

            OptionsPriceText.Text = "Options total: " + config.OptionsTotal.ToString("N0") + " RUB";

            TotalPriceText.Text = "TOTAL: " + config.TotalPrice.ToString("N0") + " RUB";
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            ((MainWindow)Application.Current.MainWindow).SetStep(2);
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Step4Page());
            ((MainWindow)Application.Current.MainWindow).SetStep(4);
        }
    }
}
