using BooksKeeper.Domain.Exceptions.BookExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Entities
{
    public class Book
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public int Year { get; private set; }

        private Book(int id, string title, string author, int year)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
        }

        /* Что касается валидации: кидать исключения с основного конструктора - не очень хорошая затея;
         * доменный объект сам отвечает за свою целостность и валидация здесь - последний рубеж валидации,
         * несмотря на то, что данные могут валидировать уровни выше (сервис, валидация DTO и тд). */
        public static Book Create(int id, string title, string author, int year)
        {
            ValidateParameters(id, title, author, year); // вынесем валидацию в отдельной метод

            return new Book(id, title, author, year);
        }

        public void ChangeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidBookTitleException("The book title cannot be empty.");

            Title = title;
        }

        public void ChangeYear(int year)
        {
            if(year < 0)
                throw new InvalidBookYearException("The year of the book cannot be negative.");

            Year = year;
        }

        public void ChangeAuthor(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new InvalidBookAuthorException("The author of the book must be filled in.");

            Author = author;
        }

        private static void ValidateParameters(int id, string title, string author, int year)
        {
            if (id < 0)
                throw new InvalidBookIdException("Book ID cannot be negative.");

            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidBookTitleException("The book title cannot be empty.");

            if (string.IsNullOrWhiteSpace(author))
                throw new InvalidBookAuthorException("The author of the book must be filled in.");

            if (year < 0)
                throw new InvalidBookYearException("The year of the book cannot be negative.");
        }
    }
}
