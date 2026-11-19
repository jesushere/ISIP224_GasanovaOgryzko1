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
using WpfApp.Pages;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CarConfiguration Configuration { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Configuration = new CarConfiguration();

            MainFrame.Navigate(new Page1(Configuration));
            SetStep(1);
        }

        public void SetStep(int step)
        {
            StepProgressBar.Value = step;
            StepTextBlock.Text = $"Step {step} of 5";
        }
    }
}