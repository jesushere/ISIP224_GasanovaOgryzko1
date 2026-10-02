/*создать конс-е прил-е для учета книг в библ-ке
у книги должны быть:
-уникал-й идентификатор (генерир-ся автоматич-ки при добав-ии)
-название
-автор
-жанр(не мене 3-х и что-то про из заданных в коде)
-год издания 
-цена

что можно деталать:
-добавлять книу(запросить параметры у юзера, идентиф-р назн-ся автомат-ки)
-удалалять книгу по идент-ру
-находить книги(по назв-ю, жанру, должны быть все варианты писка) и выводить всю инфу
-сортировать книги по назв-ю/году(обе команды)
-выводить самую рич и самую чип книгу
-группировать по авторам и выводить кол-во книг у каждого автора

юзать LINQ, список запол-ть 5 тест-ми д-ми, проверка всех возм-х  зн-й*/

using System;
using System.Collections.Generic;
using System.Linq;

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
        static void Main()
        {

        }
    }
}