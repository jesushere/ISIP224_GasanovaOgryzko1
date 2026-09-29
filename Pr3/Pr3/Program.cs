using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string text = ReadText();
        Console.WriteLine("длина текста: " + text.Length);
    }

    static string ReadText()
    {
        while (true)
        {
            Console.WriteLine("введите текст:");
            string input = Console.ReadLine();

            if (input != null && input.Length >= 100)
            {
                return input;
            }

            Console.WriteLine("ошибка! введите не менее 100 символов");
        }
    }
}