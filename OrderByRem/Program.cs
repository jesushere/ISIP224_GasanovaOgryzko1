
using System;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace OrderByRem
{
    class Program
    {
        static void PrintLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static string connectionString = @"Server=localhost\SQLEXPRESS02;" + "Database=OrderByRemDatabase;" + "Integrated Security=True;" + "TrustServerCertificate=True;";

        //workin' on registration n loggin' into the acc
        static int currentUserStatus = 0; //indicate that the user hasn't yet logged in to the acc
        static string currentUserName = "";

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n                               ORDER BY REM");
                Console.WriteLine("                      ---------------------------------");
                Console.WriteLine("                             the resale place");
                Console.WriteLine("\n1. view all products");
                Console.WriteLine("2. view products by category");
                Console.WriteLine("3. sing up");
                Console.WriteLine("4. login");
                Console.WriteLine("5. logout");
                Console.WriteLine("6. view cart");
                Console.WriteLine("7. add product to cart");
                Console.WriteLine("8. remove product from cart");
                Console.WriteLine("9. buy one product");
                Console.WriteLine("10. buy entire cart");
                Console.WriteLine("11. order history");
                Console.WriteLine("0. exit");
                Console.Write("\nchoose an option: ");

                string choise = Console.ReadLine();

                if (currentUserStatus != 0)
                {
                    PrintLine($"you logged in as {currentUserName}", ConsoleColor.DarkGray);
                    PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                    Console.ReadKey();
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
                        PrintLine("you've logged out", ConsoleColor.Green);
                        PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                        Console.ReadKey();
                        break;
                    case "6":
                        ShowCart();
                        break;
                    case "7":
                        AddToCart();
                        break;
                    case "8":
                        RemoveFromCart();
                        break;
                    case "9":
                        PurchaseProduct();
                        break;
                    case "10":
                        PurchaseCart();
                        break;
                    case "11":
                        ShowOrderHistory();
                        break;
                    case "0":
                        return;
                    default:
                        PrintLine("such a choise doesn't exist", ConsoleColor.DarkGray);
                        PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowProducts()
        {
            PrintLine("\n-PRODUCT CATALOG-", ConsoleColor.Yellow);

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
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static void ShowProductByCategory()
        {
            PrintLine("\n-CATEGORIES-", ConsoleColor.Yellow);

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
                        PrintLine("error! enter a valid value", ConsoleColor.Red);
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
                                    PrintLine("no products found in this category", ConsoleColor.Red);
                            }
                        }
                    }
                }
            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static void SingUpUser()
        {
            PrintLine("\n-REGISTRATION-", ConsoleColor.Yellow);

            Console.Write("enter username: ");
            string userName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userName))
            {
                PrintLine("username cannot be empty!", ConsoleColor.Red);
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
                            PrintLine("this username is already taken!", ConsoleColor.Red);
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
                PrintLine("password don't mutch!", ConsoleColor.Red);
                return;
            }

            if (password.Length < 8)
            {
                PrintLine("password must contain at least 8 characters!", ConsoleColor.Red);
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

                        PrintLine("successful registration!", ConsoleColor.Green);
                    }
                }
            }

            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                PrintLine("this username is already taken", ConsoleColor.Red);
            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
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
            PrintLine("\n-LOGIN-", ConsoleColor.Yellow);

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
                                PrintLine("stored password data is invalid", ConsoleColor.Red);
                                return;
                            }

                            byte[] salt = Convert.FromBase64String(parts[1]);
                            byte[] expectedHash = Convert.FromBase64String(parts[2]);

                            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);

                            if (!CryptographicOperations.FixedTimeEquals(actualHash, expectedHash))
                            {
                                PrintLine("invalid username or password", ConsoleColor.Red);
                                return;
                            }

                            currentUserStatus = userId;
                            currentUserName = storedUserName;

                            EnsureCart(currentUserStatus);

                            PrintLine($"welcome back, {currentUserName}!", ConsoleColor.Green);
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
                PrintLine("stored password data is invalid", ConsoleColor.Red);
            }
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
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

                    using (SqlCommand command = new SqlCommand(request, connection))
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
                PrintLine("you must be logged in to add products to the cart", ConsoleColor.Red);
                PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                Console.ReadKey();
                return;
            }

            PrintLine("\n-ADD TO CART-", ConsoleColor.Yellow);

            Console.Write("enter product ID to add to cart: ");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                PrintLine("error! enter a valid value", ConsoleColor.Red);
                return;
            }
            string request = @"INSERT INTO CartItem (CartId, ProductId, AddedAt)
                   SELECT Cart.Id, Product.Id, GETDATE()
                   FROM Cart
                   INNER JOIN Product
                       ON Product.Id = @ProductId
                   WHERE Cart.UserId = @UserId
                   AND NOT EXISTS (
                       SELECT 1
                       FROM CartItem
                       WHERE CartItem.CartId = Cart.Id
                       AND CartItem.ProductId = Product.Id
                   )
                   AND NOT EXISTS (
                       SELECT 1
                       FROM OrderItem
                       WHERE OrderItem.ProductId = Product.Id
                   );";
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
                            PrintLine("product added to cart successfully!", ConsoleColor.Green);
                        }
                        else
                        {
                            PrintLine("failed to add product to cart. make sure the product ID is correct", ConsoleColor.Red);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static void ShowCart()
        {
            if (currentUserStatus == 0)
            {
                PrintLine("you must be logged in to view the cart", ConsoleColor.Red);
                PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                Console.ReadKey();
                return;
            }
            PrintLine("\n-CART-", ConsoleColor.Yellow);
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
                                Console.WriteLine($"product: {reader["ProductName"]}");
                                Console.WriteLine($"category: {reader["CategoryName"]}");
                                Console.WriteLine($"price: {price:F2} RUB");
                                Console.WriteLine($"added: {reader["AddedAt"]}");
                            }

                            if (!found)
                            {
                                PrintLine("your cart is empty", ConsoleColor.DarkGray);
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
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static void RemoveFromCart()
        {
            if (currentUserStatus == 0)
            {
                PrintLine("you must be logged in to remove products from the cart", ConsoleColor.Red);
                PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                Console.ReadKey();
                return;
            }
            PrintLine("\n-REMOVE FROM CART-", ConsoleColor.Yellow);
            Console.Write("enter product ID to remove from cart: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                PrintLine("error! enter a valid value", ConsoleColor.Red);
                return;
            }
            string request = @"DELETE CartItem
                               FROM CartItem
                               INNER JOIN Cart
                                   ON CartItem.CartId = Cart.Id
                               WHERE Cart.UserId = @UserId
                               AND CartItem.ProductId = @ProductId;";
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
                            PrintLine("product removed from cart successfully!", ConsoleColor.Green);
                        }
                        else
                        {
                            PrintLine("failed to remove product from cart. make sure the product ID is correct and exists in your cart", ConsoleColor.Red);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static int ChoosePickupPoint()
        {
            PrintLine("\n-PICKUP POINTS-", ConsoleColor.Yellow);

            string query = @"SELECT Id, PointName, Address
                             FROM PickupPoint
                             ORDER BY Id;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(
                                $"{reader["Id"]}. {reader["PointName"]}");

                            Console.WriteLine(
                                $"   Address: {reader["Address"]}");
                        }
                    }
                }

                Console.Write("\nchoose pickup point ID: ");

                if (!int.TryParse(Console.ReadLine(), out int pointId))
                {
                    PrintLine("invalid ID!", ConsoleColor.Red);
                    return 0;
                }

                return pointId;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }

        static void PurchaseProduct()
        {
            if (currentUserStatus == 0)
            {
                PrintLine("you must be logged in to purchase products", ConsoleColor.Red);
                PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                Console.ReadKey();
                return;
            }

            PrintLine("\n-PURCHASE PRODUCT-", ConsoleColor.Yellow);
            Console.Write("enter product ID: ");

            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                PrintLine("invalid product ID!", ConsoleColor.Red);
                return;
            }

            int pickupPointId = ChoosePickupPoint();

            if (pickupPointId <= 0)
                return;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //checking pickup point
                        string checkPoint = @"SELECT COUNT(*)
                                              FROM PickupPoint
                                              WHERE Id = @PointId;";

                        using (SqlCommand command = new SqlCommand(checkPoint, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@PointId", pickupPointId);

                            if ((int)command.ExecuteScalar() == 0)
                            {
                                Console.WriteLine("pickup point not found!");
                                transaction.Rollback();
                                return;
                            }
                        }

                        //creating order
                        string orderQuery = @"INSERT INTO [Order] (UserId, PickupPointId, CreatedAt)
                                              VALUES (@UserId, @PointId, GETDATE());
                                              SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        int orderId;

                        using (SqlCommand command = new SqlCommand(orderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", currentUserStatus);
                            command.Parameters.AddWithValue("@PointId", pickupPointId);

                            orderId = (int)command.ExecuteScalar();
                        }

                        //adding product to order
                        string itemQuery = @"INSERT INTO OrderItem (OrderId, ProductId, UnitPriceAtPurchase)
                                             SELECT @OrderId, Product.Id, Product.Price
                                             FROM CartItem
                                             INNER JOIN Cart
                                                 ON CartItem.CartId = Cart.Id
                                             INNER JOIN Product
                                                 ON CartItem.ProductId = Product.Id
                                             WHERE Cart.UserId = @UserId
                                             AND Product.Id = @ProductId AND NOT EXISTS
                                             (SELECT 1 FROM OrderItem WHERE OrderItem.ProductId = Product.Id);";

                        int rowsAffected;

                        using (SqlCommand command = new SqlCommand(itemQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.Parameters.AddWithValue("@UserId", currentUserStatus);
                            command.Parameters.AddWithValue("@ProductId", productId);

                            rowsAffected = command.ExecuteNonQuery();
                        }

                        if (rowsAffected == 0)
                        {
                            transaction.Rollback();

                            PrintLine("product is not in your cart or has already been sold!", ConsoleColor.DarkGray);
                            return;
                        }

                        //removing product from cart
                        string deleteQuery = @"DELETE CartItem
                                               FROM CartItem
                                               INNER JOIN Cart
                                                   ON CartItem.CartId = Cart.Id
                                               WHERE Cart.UserId = @UserId
                                               AND CartItem.ProductId = @ProductId;";

                        using (SqlCommand command = new SqlCommand(deleteQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", currentUserStatus);
                            command.Parameters.AddWithValue("@ProductId", productId);

                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        PrintLine("\npurchase successful!", ConsoleColor.Green);
                        Console.WriteLine($"order ID: {orderId}");
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                    PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                    Console.ReadKey();
                }
            }
        }

        static void PurchaseCart()
        {
            if (currentUserStatus == 0)
            {
                PrintLine("you must be logged in to purchase products", ConsoleColor.Red);
                return;
            }
            PrintLine("\n-PURCHASE ENTIRE CART-", ConsoleColor.Yellow);
            int pickupPointId = ChoosePickupPoint();
            if (pickupPointId <= 0)
                return;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //checking pickup point
                        string checkPoint = @"SELECT COUNT(*)
                                                      FROM PickupPoint
                                                      WHERE Id = @PointId;";
                        using (SqlCommand command = new SqlCommand(checkPoint, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@PointId", pickupPointId);
                            if ((int)command.ExecuteScalar() == 0)
                            {
                                PrintLine("pickup point not found!", ConsoleColor.Red);
                                transaction.Rollback();
                                return;
                            }
                        }
                        //creating order
                        string orderQuery = @"INSERT INTO [Order] (UserId, PickupPointId, CreatedAt)
                                                      VALUES (@UserId, @PointId, GETDATE());
                                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";
                        int orderId;
                        using (SqlCommand command = new SqlCommand(orderQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", currentUserStatus);
                            command.Parameters.AddWithValue("@PointId", pickupPointId);
                            orderId = (int)command.ExecuteScalar();
                        }
                        //adding products to order
                        string itemQuery = @"INSERT INTO OrderItem (OrderId, ProductId, UnitPriceAtPurchase)
                                                     SELECT @OrderId, Product.Id, Product.Price
                                                     FROM CartItem
                                                     INNER JOIN Cart
                                                         ON CartItem.CartId = Cart.Id
                                                     INNER JOIN Product
                                                         ON CartItem.ProductId = Product.Id
                                                     WHERE Cart.UserId = @UserId AND NOT EXISTS
                                                     (SELECT 1 FROM OrderItem WHERE OrderItem.ProductId = Product.Id);";
                        int addedItems;

                        using (SqlCommand command = new SqlCommand(itemQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.Parameters.AddWithValue("@UserId", currentUserStatus);
                            addedItems = command.ExecuteNonQuery();
                        }

                        //checking whether all products were available
                        string countQuery = @"SELECT COUNT(*)
                                                      FROM CartItem
                                                      INNER JOIN Cart
                                                          ON CartItem.CartId = Cart.Id
                                                      WHERE Cart.UserId = @UserId;";

                        int cartCount;

                        using (SqlCommand command = new SqlCommand(countQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", currentUserStatus);
                            cartCount = (int)command.ExecuteScalar();
                        }

                        if (cartCount == 0 || addedItems != cartCount)
                        {
                            transaction.Rollback();

                            PrintLine("cart is empty or some products have already been sold!", ConsoleColor.DarkGray);

                            return;
                        }

                        //clearing cart
                        string deleteQuery = @"DELETE CartItem
                                                       FROM CartItem
                                                       INNER JOIN Cart
                                                           ON CartItem.CartId = Cart.Id
                                                       WHERE Cart.UserId = @UserId;";

                        using (SqlCommand command = new SqlCommand(deleteQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@UserId", currentUserStatus);

                            command.ExecuteNonQuery();
                        }

                        //calculating total
                        string totalQuery = @"SELECT SUM(UnitPriceAtPurchase)
                                                      FROM OrderItem
                                                      WHERE OrderId = @OrderId;";

                        decimal total;

                        using (SqlCommand command = new SqlCommand(totalQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            total = Convert.ToDecimal(command.ExecuteScalar());
                        }

                        transaction.Commit();

                        PrintLine("\npurchase successful!", ConsoleColor.Green);
                        Console.WriteLine($"order ID: {orderId}");
                        Console.WriteLine($"products purchased: {addedItems}");
                        Console.WriteLine($"total: {total:F2} RUB");
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.Message);
                    }
                    PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                    Console.ReadKey();
                }
            }
        }
            static void ShowOrderHistory()
            {
                if (currentUserStatus == 0)
                {
                    PrintLine("you must be logged in to see order history", ConsoleColor.Red);
                    PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
                    Console.ReadKey();
                    return;
                }

                PrintLine("\n-ORDER HISTORY-", ConsoleColor.Yellow);
                Console.WriteLine("1. sort by date: ascending");
                Console.WriteLine("2. sort by date: descending");
                Console.Write("\nchoose sorting option: ");

                string choice = Console.ReadLine();

                string sorting;

                if (choice == "1")
                {
                    sorting = "ASC";
                }
                else if (choice == "2")
                {
                    sorting = "DESC";
                }
                else
                {
                    PrintLine("invalid choice!", ConsoleColor.Red);
                    return;
                }

                string request = $@"SELECT [Order].Id AS OrderId, [Order].CreatedAt, PickupPoint.PointName, PickupPoint.Address, Product.Id AS ProductId, Product.ProductName, OrderItem.UnitPriceAtPurchase,
                                    SUM(OrderItem.UnitPriceAtPurchase)
                                    OVER (PARTITION BY [Order].Id) AS OrderTotal
                                    FROM [Order]
                                    INNER JOIN PickupPoint
                                        ON [Order].PickupPointId = PickupPoint.Id
                                    INNER JOIN OrderItem
                                        ON [Order].Id = OrderItem.OrderId
                                    INNER JOIN Product
                                        ON OrderItem.ProductId = Product.Id
                                    WHERE [Order].UserId = @UserId
                                    ORDER BY [Order].CreatedAt {sorting}, [Order].Id, Product.Id;";

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
                            bool found = false;
                            int currentOrderId = -1;

                            while (reader.Read())
                            {
                                found = true;

                                int orderId = Convert.ToInt32(reader["OrderId"]);

                                if (orderId != currentOrderId)
                                {
                                    currentOrderId = orderId;

                                    Console.WriteLine($"ORDER #{orderId}");

                                    DateTime date = Convert.ToDateTime(reader["CreatedAt"]);

                                    Console.WriteLine($"date: {date:dd.MM.yyyy HH:mm}");

                                    Console.WriteLine($"pickup point: {reader["PointName"]}");

                                    Console.WriteLine($"address: {reader["Address"]}");

                                    decimal orderTotal = Convert.ToDecimal(reader["OrderTotal"]);

                                    Console.WriteLine($"TOTAL: {orderTotal:F2} RUB");
                                }

                                decimal price = Convert.ToDecimal(reader["UnitPriceAtPurchase"]);

                                Console.WriteLine($"product ID: {reader["ProductId"]}");

                                Console.WriteLine($"product: {reader["ProductName"]}");

                                Console.WriteLine($"price: {price:F2} RUB");
                            }

                            if (!found)
                            {
                                PrintLine("you haven't made any purchases yet", ConsoleColor.DarkGray);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            PrintLine("\npress any key to return to the main menu...", ConsoleColor.DarkGray);
            Console.ReadKey();
        }
    }
}

//refine the visual presentation of the info
