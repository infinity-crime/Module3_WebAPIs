using BooksKeeper.Domain.Common;
using BooksKeeper.Domain.Exceptions.BookExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Entities
{
    public class Book : BaseEntity<Guid>
    {
        public string Title { get; private set; }
        public int Year { get; private set; }

        public ICollection<Author> Authors { get; set; } = new List<Author>();

        private Book() { }

        /* Что касается валидации: кидать исключения с основного конструктора - не очень хорошая затея;
         * доменный объект сам отвечает за свою целостность и валидация здесь - последний рубеж валидации,
         * несмотря на то, что данные могут валидировать уровни выше (сервис, валидация DTO и тд). */
        public static Book Create(string title, int year)
        {
            ValidateParameters(title, year); // вынесем валидацию в отдельной метод

            return new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Year = year
            };
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

        private static void ValidateParameters(string title, int year)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new InvalidBookTitleException("The book title cannot be empty.");

            if (year < 0)
                throw new InvalidBookYearException("The year of the book cannot be negative.");
        }
    }
}
