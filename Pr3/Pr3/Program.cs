using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<TextStats> history = new List<TextStats>();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. ввести новый текст");
            Console.WriteLine("2. показать статистику прошлых текстов");
            Console.WriteLine("0. выход");
            string choise = Console.ReadLine();

            if (choise == "1")
            {
                string text = ReadText();
                TextStats stats = Analyze(text);
                history.Add(stats);
                PrintStats(stats);
            }

            else if (choise == "2")
            {
                if (history.Count == 0)
                {
                    Console.WriteLine("пока пусто");
                }

                else
                {
                    for (int i = 0; i < history.Count; i++)
                    {
                        Console.WriteLine();
                        Console.WriteLine((i+1) + " текст");
                        PrintStats(history[i]);
                    }
                }
            }

            else if (choise == "0")
            {
                break;
            }

            else
            {
                Console.WriteLine("ЗАПУЩЕНО УДАЛЕНИЕ ПАПКИ System32!!!!!!!");
            }
        }
    
    
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

    static void CountVowelsAndConsonants(string text, out int vowels, out int consonants)
    {
        string vowelLetters = "уеаояиёыэeyuioa";
        vowels = 0;
        consonants = 0;

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char lower = char.ToLower(c);
                if (vowelLetters.Contains(lower))
                {
                    vowels++;
                }
                else
                {
                    consonants++;
                }
            }
        }
    }

    //юзаем словарь, потому что он удобнее списка или массива
    static Dictionary<char, int> CountLetterFrequency(string text) //тут у нас ключ это чар(буква), а инт это значение(сколько раз она встречается короче)
    {
        Dictionary<char, int> freq = new Dictionary<char, int>();

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char lower = char.ToLower(c);
                if (freq.ContainsKey(lower))
                {
                    freq[lower]++;
                }

                else
                {
                    freq[lower] = 1;
                }
            }
        }

        return freq;
    }

    static void PrintLetterFrequency(Dictionary<char, int> freq)
    {
        foreach (KeyValuePair<char, int> pair in freq)
        {
            Console.Write(pair.Key + "-" + pair.Value + ", ");
        }

        Console.WriteLine();
    }

    static TextStats Analyze(string text)
    {
        TextStats stats = new TextStats();
        List<string> words = GetWords(text);

        stats.Text = text;
        stats.WordCount = words.Count;
        stats.Shortest = FindShortestWord(words);
        stats.Longest = FindLongestWord(words);
        stats.SentenceCount = CountSentences(text);
        CountVowelsAndConsonants(text, out stats.Vowels, out stats.Consonants);
        stats.Frequency = CountLetterFrequency(text);

        return stats;
    }

    static void PrintStats(TextStats s)
    {
        Console.WriteLine("всего слов: " + s.WordCount);
        Console.WriteLine("самое короткое слово: " + s.Shortest);
        Console.WriteLine("самое  длинное слово: " + s.Longest);
        Console.WriteLine("кол-во предложений: " + s.SentenceCount);
        Console.WriteLine("колво гласных: " + s.Vowels + ", согласных: " + s.Consonants);
        Console.WriteLine("частота букв: ");
        PrintLetterFrequency(s.Frequency);
    }
}

//зафиксируем статистику одного текста сначала
class TextStats
{
    public string Text;
    public int WordCount;
    public string Shortest;
    public string Longest;
    public int SentenceCount;
    public int Vowels;
    public int Consonants;
    public Dictionary<char, int> Frequency;
}



//Понятные инструкции сокращают число вопросов. Удобный интерфейс снижает барьеры для новичков. Балансировка распределяет запросы равномерно. Откат к предыдущей версии восстанавливает работу быстро. Понятная документация ускоряет подключение партнёров. При грамотном подходе, аккуратная работа с данными поддерживает прозрачность работы системы в повседневной эксплуатации. Распределение нагрузки поддерживает стабильную работу. На практике, регулярное обновление систем поддерживает точность обработки данных без усложнения системы.
//Porro ducimus sit cillum veniam autem sunt perferendis voluptatem maxime aute. Veniam dolor unde accusamus pariatur ex neque incididunt ullamco minima. In nemo occaecat aspernatur cupidatat recusandae alias quasi sunt. Possimus ullamco vero quasi magna pariatur iusto quis. Officia recusandae sed quo placeat deserunt eiusmod quisquam occaecat quibusdam vero nostrum occaecat. Laboris aut corporis nostrum eiusmod placeat non facere.