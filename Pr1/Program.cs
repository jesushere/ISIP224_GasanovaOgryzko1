using System;
using System.Linq;

namespace ISIP224_GasanovaOgryzko1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("введите кол-во операций: ");
            int n = Convert.ToInt32(Console.ReadLine());

            while (n < 2 || n > 40)
            {
                Console.Write("ошибка!! введите число от 2 до 40: ");
                n = Convert.ToInt32(Console.ReadLine());
            }

            string[] names = new string[n];
            double[] prices = new double[n];

            for (int i = 0; i < n; i++)
            {
                Console.Write("введите название услуги или товара: ");
                names[i] = Console.ReadLine();

                Console.Write("введите стоимость в рублях: ");
                prices[i] = Convert.ToDouble(Console.ReadLine());
            }

            while (true)
            {
                Console.WriteLine("1.вывод данных");
                Console.WriteLine("2.статистика");
                Console.WriteLine("3.сортировка по цене");
                Console.WriteLine("4.конвертация валюты");
                Console.WriteLine("5.поиск по названию");
                Console.WriteLine("0.выход");

                int choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 0)
                {
                    break;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\nрасходы:");

                        for (int i = 0; i < n; i++)
                        {
                            Console.WriteLine($"{names[i]} - {prices[i]} руб.");
                        }

                        break;

                    case 2:
                        double sum = prices.Sum();
                        double average = prices.Average();
                        double max = prices.Max();
                        double min = prices.Min();

                        Console.WriteLine("\nстатистика:");
                        Console.WriteLine($"сумма: {sum} руб.");
                        Console.WriteLine($"среднее: {average:F2} руб.");
                        Console.WriteLine($"саксимальная трата: {max} руб.");
                        Console.WriteLine($"синимальная трата: {min} руб.");

                        break;


                    case 3:

                        for (int i = 0; i < n - 1; i++)
                        {
                            for (int j = 0; j < n - 1 - i; j++)
                            {
                                if (prices[j] > prices[j + 1])
                                {
                                    double tempPrice = prices[j];
                                    prices[j] = prices[j + 1];
                                    prices[j + 1] = tempPrice;

                                    string tempName = names[j];
                                    names[j] = names[j + 1];
                                    names[j + 1] = tempName;
                                }
                            }
                        }


                        for (int i = 0; i < n; i++)
                        {
                            Console.WriteLine($"{names[i]} - {prices[i]} руб.");
                        }

                        break;


                    case 4:
                        Console.WriteLine("\nвыберите валюту:");
                        Console.WriteLine("1.доллар");
                        Console.WriteLine("2.евро");
                        Console.WriteLine("3.свой курс");

                        int currency = Convert.ToInt32(Console.ReadLine());

                        double rate = 1;
                        string currencyName = "";

                        if (currency == 1)
                        {
                            Console.Write("введите курс доллара: ");
                            rate = Convert.ToDouble(Console.ReadLine());
                            currencyName = "дол.";
                        }
                        else if (currency == 2)
                        {
                            Console.Write("введите курс евро: ");
                            rate = Convert.ToDouble(Console.ReadLine());
                            currencyName = "евро";
                        }
                        else if (currency == 3)
                        {
                            Console.Write("введите свой курс: ");
                            rate = Convert.ToDouble(Console.ReadLine());
                            currencyName = "у.е.";
                        }

                        for (int i = 0; i < n; i++)
                        {
                            double result = prices[i] / rate;

                            Console.WriteLine($"{names[i]} - {result:F2} {currencyName}");
                        }

                        break;


                    case 5:
                        Console.Write("введите название для поиска: ");
                        string search = Console.ReadLine();

                        bool found = false;

                        for (int i = 0; i < n; i++)
                        {
                            if (names[i].ToLower().Contains(search.ToLower()))
                            {
                                Console.WriteLine($"{names[i]} - {prices[i]} руб.");
                                found = true;
                            }
                        }

                        if (!found)
                        {
                            Console.WriteLine("не найдено");
                        }

                        break;


                    default:
                        Console.WriteLine("такого пункта нет");
                        break;
                }
            }
        }
    }
}
