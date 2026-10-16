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

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1.добавить товар");
                Console.WriteLine("2.удалить товар");
                Console.WriteLine("3.заказать поставку");
                Console.WriteLine("4.продать товар");
                Console.WriteLine("5.найти товар");
                Console.WriteLine("0.выход");

                Console.Write("выберите команду: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProduct(products);
                        break;

                    case "2":
                        DeleteProducts(products);
                        break;

                    case "3":
                        SupplyProducts(products);
                        break;

                    case "4":
                        SellProduct(products);
                        break;

                    case "5":
                        SearchProduct(products);
                        break;

                    case "0":
                        Console.WriteLine("программа завершена.");
                        return;

                    default:
                        Console.WriteLine("такой команды нет");
                        break;
                }
            }


            Console.ReadKey();
        }

        static void AddProduct(List<Product> products)
        {
            Console.WriteLine("\nдобавление нового товара");
            Console.WriteLine();

            string name;

            while (true)
            {
                Console.Write("введите название товара: ");
                name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ошибка!! название не может быть пустым");
                }
            }

            double price;

            while (true)
            {
                Console.Write("введите цену: ");
                if (double.TryParse(Console.ReadLine(), out price) && price > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ошибка!! цена должна быть положительным числом");
                }
            }

            int quantity;

            while (true)
            {
                Console.Write("введите количество: ");
                if (int.TryParse(Console.ReadLine(), out quantity) && quantity >= 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ошибка!! количество должно быть неотрицательным");
                }
            }

            int categoryNumber;

            while (true)
            {
                Console.WriteLine("выберите категорию:");
                Console.WriteLine("1. еда");
                Console.WriteLine("2. одежда");
                Console.WriteLine("3. электроника");
                Console.Write("введите номер категории: ");
                if (int.TryParse(Console.ReadLine(), out categoryNumber) && categoryNumber >= 1 && categoryNumber <= 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("ошибка!! введите число от 1 до 3");
                }
            }

            Category category;

            if (categoryNumber == 1)
            {
                category = Category.Food;
            }
            else if (categoryNumber == 2)
            {
                category = Category.Clothes;
            }
            else
            {
                category = Category.Electronics;
            }

            int code = 1;

            foreach (Product product in products)
            {
                if (product.Code >= code)
                {
                    code = product.Code + 1;
                }
            }

            Product newProduct = new Product(code, name, price, quantity, category);

            products.Add(newProduct);

            Console.WriteLine();
            Console.WriteLine("товар добавлен");
            Console.WriteLine($"код: {newProduct.Code}, название: {newProduct.Name}, цена: {newProduct.Price} руб., количество: {newProduct.Quantity}, категория: {newProduct.Category}, в наличии: {newProduct.IsInStock}");
        }

        static void DeleteProducts(List<Product> products)
        {

            Console.WriteLine();
            Console.WriteLine("\nудаление товаров");

            Console.WriteLine("введите код товара: ");
            int code;

            while (!int.TryParse(Console.ReadLine(), out code) || code <=0)
            {
                Console.WriteLine("ошибка!! введите положительное число");
                Console.WriteLine("введите код товара: ");
            }

            Product product = null;

            foreach (Product item in products)
            {
                if (item.Code == code)
                {
                    product = item;
                    break;
                }
            }

            if (product == null)
            {
                Console.WriteLine("товар с таким кодом не найден");
                return;
            }

            products.Remove(product);
            Console.WriteLine($"товар {product.Name} удален");

        }

        static void SupplyProducts(List<Product> products)
        {
            Console.WriteLine();
            Console.WriteLine("поставка товара");

            Console.WriteLine("введите код товара: ");

            int code;

            while (!int.TryParse(Console.ReadLine(), out code) || code <= 0)

                Console.WriteLine("введите положительное значение");
            Console.WriteLine("введите код товара: ");

            Product product = null;

            foreach (Product item in products)
            {
                if (item.Code == code)
                {
                    product = item;
                    break;
                }
            }

            if (product == null)
            {
                Console.WriteLine("товар с таким кодом не найден");
                return;
            }

            int quantity;

            while (true)
            {
                Console.Write("введите кол-во товара для поставки: ");

                if (int.TryParse(Console.ReadLine(), out quantity) && quantity>0)
                    break;
                Console.WriteLine("значение должно быть больше 0");
            }

            product.Quantity += quantity;
            product.IsInStock = product.Quantity > 0;

            Console.WriteLine($"поставка выполнена");
            Console.WriteLine($"товар: {product.Name}");
            Console.WriteLine($"новое кол-во: {product.Quantity}"); ///господииииииииии

        }

        static void SellProduct(List<Product> products)
        {
            Console.WriteLine();
            Console.WriteLine("продажа товара");

            Console.Write("введите код товара: ");

            int code;

            while (!int.TryParse((string)Console.ReadLine(), out code) || code <= 0)
            {
                Console.WriteLine("введите положительное значение");
                Console.WriteLine("введите код товара: ");
            }

            Product product = null;

            foreach (Product item in products)
            {
                if (item.Code == code)
                {
                    product = item;
                    break;
                }
            }
            if (product == null)
            {
                Console.WriteLine("товар с таким кодом не найден");
                return;
            }

            int quantity;

            while (true)
            {
                Console.Write("введите кол-во товаров для продажи: ");

                if (int.TryParse(Console.ReadLine(), out quantity) && quantity>0)
                    break;
                Console.WriteLine("значение должно быть больше 0");
            }

            if (quantity > product.Quantity)
            {
                Console.WriteLine("недостаточно товара на складе"); //вот тут стоп
                Console.WriteLine($"доступно: {product.Quantity}");
                return;
            }

            product.Quantity -= quantity;
            if (product.Quantity == 0)
            {
                product.IsInStock = false;
            }

            Console.WriteLine("продажа выполнена");
            Console.WriteLine($"товар: {product.Name}");
            Console.WriteLine($"осталось на складе: {product.Quantity}");
        }

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"код: {product.Code}");
            Console.WriteLine($"название: {product.Name}");
            Console.WriteLine($"цена: {product.Price} руб.");
            Console.WriteLine($"кол-во: {product.Quantity}");
            Console.WriteLine($"на складе: {(product.IsInStock ? "да" : "нет")}");
            Console.WriteLine($"категория: {product.Category}");
        }

        static void SearchProduct(List<Product> products)
        {
            Console.WriteLine();
            Console.WriteLine("поиск товара");
            Console.WriteLine("1.поиск по коду");
            Console.WriteLine("2.поиск по названию");
            Console.WriteLine("3.поиск по категории");

            Console.WriteLine("выберите способ поиска: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("введите код товара: ");
                int code;

                if (!int.TryParse(Console.ReadLine(), out code) || code <= 0)
                {
                    Console.WriteLine("Неверный код.");
                    return;
                }

                Product product = null;

                foreach (Product item in products)
                {
                    if (item.Code == code)
                    {
                        product = item;
                        break;
                    }
                }

                if (product != null)
                {
                    ShowProduct(product);
                }
                else
                {
                    Console.WriteLine("товар не найден.");
                }
            }
            else if (choice == "2")
            {
                Console.Write("введите название товара: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("название не может быть пустым");
                    return;
                }

                bool found = false;

                foreach (Product product in products)
                {
                    if (product.Name.ToLower().Contains(name.ToLower()))
                    {
                        ShowProduct(product);
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("товары не найдены");
                }
            }
            else if (choice == "3")
            {
                Console.WriteLine("1.продукты");
                Console.WriteLine("2.одежда");
                Console.WriteLine("3.электроника");

                Console.Write("выберите категорию: ");
                int categoryNumber;

                if (!int.TryParse(Console.ReadLine(), out categoryNumber) ||
                    categoryNumber < 1 || categoryNumber > 3)
                {
                    Console.WriteLine("неверная категория");
                    return;
                }

                Category category;

                if (categoryNumber == 1)
                    category = Category.Food;
                else if (categoryNumber == 2)
                    category = Category.Clothes;
                else
                    category = Category.Electronics;

                bool found = false;

                foreach (Product product in products)
                {
                    if (product.Category == category)
                    {
                        ShowProduct(product);
                        found = true;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("товары этой категории не найдены");
                }
            }
            else
            {
                Console.WriteLine("такого варианта поиска нет");
            }
        }
    }
}