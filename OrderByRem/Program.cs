
using System;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace OrderByRem
{
    class Program
    {
        static string connectionString = @"Server=localhost\SQLEXPRESS02;" + "Database=OrderByRemDatabase;" + "Integrated Security=True;" + "TrustServerCertificate=True;";

        //workin' on registration n loggin' into the acc
        static int currentUserStatus = 0; //indicate that the user hasn't yet logged in to the acc
        static string currentUserName = "";

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("ORDER BY REM");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("the resale place");
                Console.WriteLine("\n1. view all products");
                Console.WriteLine("2. view products by category");
                Console.WriteLine("3. sing up");
                Console.WriteLine("4. login");
                Console.WriteLine("5. logout");
                Console.WriteLine("0. exit");
                Console.Write("\n choose an option:");

                string choise = Console.ReadLine();

                if (currentUserStatus != 0)
                {
                    Console.WriteLine($"you logged in as {currentUserName}");
                }

                switch (choise)
                {
                    case "1":
                        ShowProducts();
                        break;
                    case "2":
                        ShowProductByCategory();
                        break;
                    case "3":
                        SingUpUser();
                        break;
                    case "4":
                        LoginUser();
                        break;
                    case "5":
                        currentUserStatus = 0;
                        currentUserName = "";
                        Console.WriteLine("you've logged out");
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

        static void SingUpUser()
        {
            Console.WriteLine("-REGISTRATION-");

            Console.Write("enter username: ");
            string userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("username cannot be empty!");
                return;
            }

            string checkQuery = @"SELECT COUNT(*)
                                  FROM [User]
                                  WHERE UserName = @UserName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(checkQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            Console.WriteLine("еhis username is already taken!");
                            return;
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            Console.Write("enter password: ");
            string password = CheckPassword();

            Console.Write("confirm password: ");
            string passwordC = CheckPassword();

            if (password != passwordC)
            {
                Console.WriteLine("password don't mutch!");
                return;
            }

            if (password.Length < 8)
            {
                Console.WriteLine("password must contain at least 8 characters!");
                return;
            }

            //hashing the password
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA3_256, 32);

            string passwordHash = "100000:" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);

            string request = @"INSERT INTO [User] (UserName, PasswordHash)
                               VALUES (@UserName, @PasswordHash);
                               SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        int newUserId = (int)command.ExecuteScalar();

                        Console.WriteLine("successful registration!");
                    }
                }
            }

            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                Console.WriteLine("this username is already taken");
            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static string CheckPassword() //readin' the password without displayin' the char's
        {
            StringBuilder password = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Length--;
                    }

                    continue;
                }

                if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                }
            }

            return password.ToString();
        }

        static void LoginUser()
        {
            Console.WriteLine("-LOGIN-");

            Console.Write("enter username: ");
            string userName = Console.ReadLine();

            Console.Write("enter password: ");
            string password = Console.ReadLine();

            string request = @"SELECT Id,UserName, PasswordHash
                               FROM [User]
                               WHERE UserName = @UserName;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                Console.WriteLine("invalid username or password");
                                return;
                            }

                            int userId = reader.GetInt32(0);
                            string storedUserName = reader.GetString(1);
                            string storedHash = reader.GetString(2);

                            string[] parts = storedHash.Split(':');

                            if (parts.Length != 3 || !int.TryParse(parts[0], out int iterations))
                            {
                                Console.WriteLine("stored password data is invalid");
                                return;
                            }

                            byte[] salt = Convert.FromBase64String(parts[1]);
                            byte[] expectedHash = Convert.FromBase64String(parts[2]);

                            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);

                            if (!CryptographicOperations.FixedTimeEquals(actualHash, expectedHash))
                            {
                                Console.WriteLine("invalid username or password");
                                return;
                            }

                            currentUserStatus = userId;
                            currentUserName = storedUserName;

                            Console.WriteLine($"welcome back, {currentUserName}!");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (FormatException)
            {
                Console.WriteLine("stored password data is invalid");
            }
        
        }
    }
}

//refine the visual presentation of the info
