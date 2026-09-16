using System;
using System.Collections.Generic;

namespace Pr2
{
    enum Category
    {
        Food,
        Clothes,
        Electronics
    }

    class Product
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsInStock { get; set; }
        public Category Category { get; set; }

        public Product(int code, string name, double price, int quantity, Category category)
        {
            Code = code;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            IsInStock = quantity > 0;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            List<Product> products = new List<Product>();

            products.Add(new Product(1, "хлеб", 80, 15, Category.Food));
            products.Add(new Product(2, "футболка", 1500, 5, Category.Clothes));
            products.Add(new Product(3, "наушники", 2500, 8, Category.Electronics));
            products.Add(new Product(4, "молоко", 100, 20, Category.Food));
            products.Add(new Product(5, "джинсы", 3500, 3, Category.Clothes));

            Console.WriteLine("\nсписок товаров:");
            foreach (Product product in products)
            {
                Console.WriteLine($"код: {product.Code}, название: {product.Name}, цена: {product.Price} руб., количество: {product.Quantity}, категория: {product.Category}, в наличии: {product.IsInStock}");
            }

            Console.WriteLine("товары добавлены");
            Console.ReadKey();
        }
    }
}