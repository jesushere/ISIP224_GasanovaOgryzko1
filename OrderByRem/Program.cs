
using System;
using Microsoft.Data.SqlClient;

namespace OrderByRem
{
    class Program
    {
        static void Main()
        {
            string connectionString = @"Server=localhost\SQLEXPRESS02;" + "Database=OrderByRemDatabase;" + "Integrated Security=True;" +  "TrustServerCertificate=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    Console.WriteLine("done");
                    Console.WriteLine($"{connection.Database}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("sql hates me");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
