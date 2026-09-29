using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string text = ReadText();
        Console.WriteLine("длина текста: " + text.Length);

        List<string> words = GetWords(text);
        Console.WriteLine("слов: " + words.Count);
        Console.WriteLine("самое короткое слово: " + FindShortestWord(words));
        Console.WriteLine("самое длинное слово: " + FindLongestWord(words));
        Console.WriteLine("всего предложений: " + CountSentences(text));
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

    static List<string> GetWords(string text)
    {
        List<string> words = new List<string>();
        string current = "";

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                current += c;
            }

            else
            {
                if (current != "")
                {
                    words.Add(current);
                    current = "";
                }
            }
        }
        if (current != "")
        {
            words.Add(current);
        }

        return words;
    }

    static string FindShortestWord(List<string> words)
    {
        string shortest = words[0];
        foreach (string w in words)
        {
            if (w.Length < shortest.Length)
            {
                shortest = w;
            }
        }

        return shortest;
    }

    static string FindLongestWord(List<string> words)
    {
        string longest = words[0];
        foreach (string w in words)
        {
            if (w.Length > longest.Length)
            {
                longest = w;
            }
        }

        return longest;
    }

    static int CountSentences(string text)
    {
        int count = 0;
        bool hasLetters= false; //это мы проверяем на всякий случай есть ли вообще буквы в самом начале предложения
        
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                hasLetters= true;
            }

            else if (c == '.' || c == '!' || c == '?')
            {
                if (hasLetters)
                {
                    count++;
                    hasLetters= false;
                }
            }
        }

        if (hasLetters)
        {
            count++;    //это если предложение не закончилось .
        }

        return count;
    }

}