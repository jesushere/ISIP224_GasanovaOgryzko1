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

namespace OnlineStore.Pages
{
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();

            CartListBox.ItemsSource = Core.Cart;

            UpdateTotal();
        }
        private void UpdateTotal()
        {
            decimal total = Core.Cart.Sum(p => p.Price);

            TotalTextBlock.Text = "TOTAL: " + total + " RUB.";
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            var product = button.DataContext as OnlineStore.Models.Product;

            if (product != null)
            {
                Core.Cart.Remove(product);
            }

            UpdateTotal();
        }
        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (Core.Cart.Count == 0)
            {
                MessageBox.Show("The cart is empty!");
                return;
            }

            NavigationService.Navigate(new OrderPage());
        }
    }
}
