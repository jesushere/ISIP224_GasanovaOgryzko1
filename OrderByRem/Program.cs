
using System;
using Microsoft.Data.SqlClient;

namespace OrderByRem
{
    class Program
    {
        static string connectionString = @"Server=localhost\SQLEXPRESS02;" + "Database=OrderByRemDatabase;" + "Integrated Security=True;" + "TrustServerCertificate=True;";
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("ORDER BY REM");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("the resale place");
                Console.WriteLine("\n1. view all products");
                Console.WriteLine("2. view products by category");
                Console.WriteLine("0. exit");
                Console.Write("\n choose an option:");

                string choise = Console.ReadLine();

                switch (choise)
                {
                    case "1":
                        ShowProducts();
                        break;
                    case "2":
                        ShowProductByCategory();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("such a choise doesn't exist");
                        break;
                }
            }
        }

        static void ShowProducts()
        {
            Console.WriteLine("-product catallog-");

            string request = @"SELECT Product.Id, Product.ProductName, Product.Description, Product.Price, Category.CategoryName
                               FROM Product
                               INNER JOIN Category
                                  ON Product.CategoryId = Category.Id
                               ORDER BY Category.CategoryName, Product.Id;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(request, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["Id"]}");
                            Console.WriteLine($"product: {reader["ProductName"]}");
                            Console.WriteLine($"category: {reader["CategoryName"]}");
                            Console.WriteLine($"price: {Convert.ToDecimal(reader["Price"]):F2} RUB");
                            Console.WriteLine($"description: {reader["Description"]}");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ShowProductByCategory()
        {
            Console.WriteLine("-CATEGORIES-");

            string categoryRequest = @"SELECT Id, CategoryName
                                       FROM Category
                                       ORDER BY Id;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(categoryRequest, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Id"]}. {reader["CategoryName"]}");
                        }
                    }

                    Console.Write("\nenter category ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int categoryId))
                    {
                        Console.WriteLine("error! enter a valid value");
                        return;
                    }

                    Console.WriteLine("CATEGORY PRODUCTS");
                    string productRequest = @"SELECT Product.Id, Product.ProductName, Product.Description, Product.Price, Category.CategoryName
                                              FROM Product
                                              INNER JOIN Category
                                                  ON Product.CategoryId = Category.Id
                                              WHERE Category.Id = @CategoryId
                                              ORDER BY Product.Id;";
                    using (SqlCommand command = new SqlCommand(productRequest, connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", categoryId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            bool found = false;

                            while (reader.Read())
                            {
                                found = true;

                                Console.WriteLine($"ID: {reader["Id"]}");
                                Console.WriteLine($"Product: {reader["ProductName"]}");
                                Console.WriteLine($"Category: {reader["CategoryName"]}");
                                Console.WriteLine($"Price: {Convert.ToDecimal(reader["Price"]):F2} RUB");
                                Console.WriteLine($"Description: {reader["Description"]}");
                            }
                            
                            if (!found)
                            {
                                Console.WriteLine("no products found in this category");
                            }
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
