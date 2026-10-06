/*создать конс-е прил-е для учета книг в библ-ке
у книги должны быть:
-уникал-й идентификатор (генерир-ся автоматич-ки при добав-ии)
-название
-автор
-жанр(не мене 3-х и что-то про из заданных в коде)
-год издания 
-цена

что можно деталать:
-добавлять книу(запросить параметры у юзера, идентиф-р назн-ся автомат-ки) done
-удалалять книгу по идент-ру done
-находить книги(по назв-ю, жанру, должны быть все варианты писка) и выводить всю инфу done
-сортировать книги по назв-ю/году(обе команды) done
-выводить самую рич и самую чип книгу
-группировать по авторам и выводить кол-во книг у каждого автора

юзать LINQ, список запол-ть 5 тест-ми д-ми, проверка всех возм-х  зн-й*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practic4
{
    enum Genre
    {
        Фантастика,
        Роман,
        Триллер,
        Детектив,
        Драма
    }

    class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public Genre Genre { get; }
        public int Year { get; }
        public decimal Price { get; }

        public Book(int id, string title, string author, Genre genre, int year, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"[{Id}] {Title} - {Author}, {Genre}, {Year} год, {Price:F2} рублей";
        }
    }

    class Program
    {
        static readonly List<Book> books = new List<Book>();

        static int nextId = 1;

        static Book AddBook(string title, string author, Genre genre, int year, decimal price)
        {
            var book = new Book(nextId++, title, author, genre, year, price);
            books.Add(book);
            return book;
        }

        static void SeedBooks()
        {
            AddBook("Трудно быть Богом", "Аркадий и Борис Стругацкие", Genre.Фантастика, 1964, 399m);
            AddBook("Проект «Аве Мария»", "Энди Вейер", Genre.Фантастика, 2021, 949m);
            AddBook("Отель с привидениями", "Уилки Коллинз", Genre.Детектив, 1878, 359m);
            AddBook("Убийства по алфавиту", "Агата Кристи", Genre.Детектив, 1936, 499m);
            AddBook("Бегущий человек", "Стивен Кинг", Genre.Триллер, 1982, 317m);
            AddBook("Ангелы и демоны", "Дэн Браун", Genre.Триллер, 2000, 579m);
            AddBook("Триумфальная арка", "Эрих Мария Ремарк", Genre.Роман, 1945, 2397m);
            AddBook("Скорбь сатаны", "Мария Корелли", Genre.Роман, 1895, 1840m);
            AddBook("Эгоист", "Джордж Мередит", Genre.Драма, 1879, 317m);
            AddBook("Дочь священника", "Джордж Оруэл", Genre.Драма, 1935, 233m);
        }

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. добавить книгу");
            Console.WriteLine("2. удалить книгу");
            Console.WriteLine("3. найти книгу");
            Console.WriteLine("4. сортировать книги по названию");
            Console.WriteLine("5. сортировать книги по году издания");
            Console.WriteLine("6. вывести самую дорогую и самую дешёвую книгу");
            Console.WriteLine("7. группировать книги по авторам");
            Console.WriteLine("0. выход");
            Console.WriteLine("ведите номер команды: ");
        }

        static void AddBookInteractive()
        {
            Console.Write("введите название книги: ");

            string title = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("значение не может быть пустым");
                return;
            }

            Console.Write("введите автора книги: ");

            string author = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(author)) {
                Console.WriteLine("значение не может быть пустым");
                return;
            }

            Console.WriteLine("доступные жанры: фантастика, роман, триллер, детектив, драма");
            Console.Write("жанр: ");

            string genreStr = Console.ReadLine()?.Trim();
            if (!Enum.TryParse(genreStr, true, out Genre genre))
            {
                Console.WriteLine("неверный жанр");
                return;
            }

            Console.Write("год издания: ");
            if(!int.TryParse(Console.ReadLine(), out int year) || year < 0 || year > DateTime.Now.Year)
            {
                Console.WriteLine("неверный год");
                return;
            }

            Console.Write("цена: ");
            if(!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("неверная цена");
                return;
            }

            var book = AddBook(title, author, genre, year, price);
            Console.WriteLine("книга добавлена: " + book);
        }

        static void DeleteBookByID()
        {
            Console.WriteLine("введите идентификатор книги для удаления: ");
            if(!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("неверный идентификатор");
                return;
            }

            var book = books.FirstOrDefault(b => b.Id == id);
            if(book == null)
            {
                Console.WriteLine("книга с таким идентификатором не найдена");
                return;
            }

            books.Remove(book);
            Console.WriteLine("книга удалена: " + book);
        }

        static void FindBooks()
        {
            Console.WriteLine("найти книгу по:");
            Console.WriteLine("1. названию");
            Console.WriteLine("2. жанру");
            Console.WriteLine("3. автору");
            Console.WriteLine("введите номер команды: ");
            string choice = Console.ReadLine();

            IEnumerable<Book> result = null;

            switch (choice)
            {
                case "1":
                    Console.Write("введите название книги: ");
                    string title = Console.ReadLine()?.Trim();
                    result = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
                    break;
                case "2":
                    Console.WriteLine("доступные жанры: фантастика, роман, триллер, детектив, драма");
                    Console.Write("жанр: ");
                    string genreStr = Console.ReadLine()?.Trim();
                    if (!Enum.TryParse(genreStr, true, out Genre genre))
                    {
                        Console.WriteLine("неверный жанр");
                        return;
                    }
                    result = books.Where(b => b.Genre == genre);
                    break;
                case "3":
                    Console.Write("введите автора книги: ");
                    string author = Console.ReadLine()?.Trim();
                    result = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
                    break;
                default:
                    Console.WriteLine("неверный выбор");
                    return;
            }

            if (result == null || !result.Any())
            {
                Console.WriteLine("книги не найдены");
                return;
            }

            Console.WriteLine("найденные книги:");
            foreach (var book in result)
            {
                Console.WriteLine(book);
            }
        }

        static void SortBooksByTitle()
        {
            var sortedBooks = books.OrderBy(b => b.Title).ToList();
            Console.WriteLine("книги отсортированы по названию:");
            foreach (var book in sortedBooks)
            {
                Console.WriteLine(book);
            }
        }

        static void SortBooksByYear()
        {
            var sortedBooks = books.OrderBy(b => b.Year).ToList();
            Console.WriteLine("книги отсортированы по году издания:");
            foreach (var book in sortedBooks)
            {
                Console.WriteLine(book);
            }
        }

            static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            SeedBooks();
            
            while (true)
            {
                ShowMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBookInteractive();
                        break;
                    case "2":
                        DeleteBookByID();
                        break;
                    case "3":
                        FindBooks();
                        break;
                    case "4":
                        SortBooksByTitle();
                        break;
                    case "5":
                        SortBooksByYear();
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("неверный выбор");
                        break;
                }
            }
        }
    }
}