using OnlineStore.Models;
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
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();

            OrderListBox.ItemsSource = Core.Cart.Select(p => p.Name + " - " + p.Price + " RUB.").ToList();

            decimal total = Core.Cart.Sum(p => p.Price);

            TotalTextBlock.Text = "TOTAL: " + total + " RUB.";
        }
        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) || string.IsNullOrWhiteSpace(EmailTextBox.Text) || string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                MessageBox.Show("Fill all fields!");
                return;
            }

            if (!EmailTextBox.Text.Contains("@"))
            {
                MessageBox.Show("Enter a valid email!");
                return;
            }

            foreach (var product in Core.Cart)
            {
                if (product.Quantity <= 0)
                {
                    MessageBox.Show("One of the products is out of stock!");
                    return;
                }
            }

            Order order = new Order
            {
                FullName = FullNameTextBox.Text,
                Email = EmailTextBox.Text,
                Address = AddressTextBox.Text,
                OrderDate = DateTime.Now,
                TotalPrice = Core.Cart.Sum(p => p.Price)
            };

            foreach (var product in Core.Cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    Price = product.Price
                });

                product.Quantity--;
            }

            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            Core.Cart.Clear();

            MessageBox.Show("Order placed successfully!");

            NavigationService.Navigate(new ProductsPage());
        }
    }
}
