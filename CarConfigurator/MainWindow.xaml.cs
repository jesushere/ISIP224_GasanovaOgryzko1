using CarConfigurator.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarConfigurator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new Step1Page());
            SetStep(1);
        }
        public void SetStep(int step)
        {
            StepProgress.Value = step;
            StepText.Text = "Step " + step + " of 5";
        }
    }
}