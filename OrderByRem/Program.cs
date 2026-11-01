
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
                Console.WriteLine("6. view cart");
                Console.WriteLine("7. add product to cart");
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
                    case "6":
                        ShowCart();
                        break;
                    case "7":
                        AddToCart();
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

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100000, HashAlgorithmName.SHA256, 32);

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
                        EnsureCart(newUserId);

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
            string password = CheckPassword();
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

                            EnsureCart(currentUserStatus);

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

        static void EnsureCart(int userID)
        {
            string request = @"IF NOT EXISTS(SELECT 1 FROM Cart WHERE UserID = @UserID)
                                             BEGIN
                                                 INSERT INTO Cart (UserID)
                                                 VALUES (@UserID);
                                             END;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command =
                        new SqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void AddToCart()
        {
            if (currentUserStatus == 0)
            {
                Console.WriteLine("you must be logged in to add products to the cart");
                return;
            }

            Console.WriteLine("\n-ADD TO CART-");

            Console.Write("enter product ID to add to cart: ");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("error! enter a valid value");
                return;
            }
            string request = @"INSERT INTO CartItem (CartId, ProductId, AddedAt)
                               SELECT Cart.Id, Product.Id, GETDATE()
                               FROM Cart
                               INNER JOIN Product
                                   ON Product.Id = @ProductId
                               WHERE Cart.UserId = @UserId
                               AND NOT EXISTS (SELECT 1 FROM CartItem WHERE CartItem.CartId = Cart.Id AND CartItem.ProductId = Product.Id);";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", currentUserStatus);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("product added to cart successfully!");
                        }
                        else
                        {
                            Console.WriteLine("failed to add product to cart. make sure the product ID is correct");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ShowCart()
        {
            if (currentUserStatus == 0)
            {
                Console.WriteLine("you must be logged in to view the cart");
                return;
            }
            Console.WriteLine("\n-CART-");
            string request = @"SELECT Product.Id, Product.ProductName, Product.Description, Product.Price, Category.CategoryName, CartItem.AddedAt
                               FROM Cart
                               INNER JOIN CartItem ON Cart.Id = CartItem.CartId
                               INNER JOIN Product ON CartItem.ProductId = Product.Id
                               INNER JOIN Category ON Product.CategoryId = Category.Id
                               WHERE Cart.UserId = @UserId
                               ORDER BY CartItem.AddedAt DESC;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(request, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", currentUserStatus);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            decimal total = 0;
                            bool found = false;

                            while (reader.Read())
                            {
                                found = true;

                                decimal price = Convert.ToDecimal(reader["Price"]);

                                total += price;

                                Console.WriteLine($"ID: {reader["Id"]}");
                                Console.WriteLine($"Product: {reader["ProductName"]}");
                                Console.WriteLine($"Category: {reader["CategoryName"]}");
                                Console.WriteLine($"Price: {price:F2} RUB");
                                Console.WriteLine($"Added: {reader["AddedAt"]}");
                            }

                            if (!found)
                            {
                                Console.WriteLine("your cart is empty");
                            }
                            else
                            {
                                Console.WriteLine($"TOTAL: {total:F2} RUB");
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

//refine the visual presentation of the info
