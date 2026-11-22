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
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();

            var electronics = Core.Context.Categories.First(c => c.Name == "Electronics");
            var accessories = Core.Context.Categories.First(c => c.Name == "Accessories");
            var other = Core.Context.Categories.First(c => c.Name == "Other");

            var products = new List<Product>
    {
            new Product { Name = "Notebook ASUS VivoBook", Price = 75990, Quantity = 5, Category = electronics },
            new Product { Name = "Headphones JBL Tune 720BT", Price = 5990, Quantity = 12, Category = electronics },
            new Product { Name = "Monitor Samsung 27 inches", Price = 18990, Quantity = 7, Category = electronics },
            new Product { Name = "Smartphone Samsung Galaxy", Price = 42990, Quantity = 6, Category = electronics },

            new Product { Name = "Mouse Logitech G102", Price = 2490, Quantity = 15, Category = accessories },
            new Product { Name = "Keyboard Redragon Kumara", Price = 3990, Quantity = 10, Category = accessories },
            new Product { Name = "Mousepad SteelSeries", Price = 1490, Quantity = 20, Category = accessories },

            new Product { Name = "USB Kingston 64 GB", Price = 990, Quantity = 25, Category = other },
            new Product { Name = "Laptop Backpack", Price = 3490, Quantity = 8, Category = other },
            new Product { Name = "Xiaomi Desk Lamp", Price = 2790, Quantity = 9, Category = other }
    };

            foreach (var product in products)
            {
                if (!Core.Context.Products.Any(p => p.Name == product.Name))
                {
                    Core.Context.Products.Add(product);
                }
            }

            Core.Context.SaveChanges();

            ProductsItemsControl.ItemsSource = Core.Context.Products.ToList();
        }
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Product product = (Product)button.DataContext;

            if (product.Quantity <= 0)
            {
                MessageBox.Show("The product is out of stock!");
                return;
            }

            Core.Cart.Add(product);

            MessageBox.Show("The product has been added to the cart!");
        }

    }
}
