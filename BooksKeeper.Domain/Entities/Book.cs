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

        private readonly List<Author> _authors = new List<Author>();
        public IReadOnlyCollection<Author> Authors => _authors.AsReadOnly();

        private Book() { }

        /* Что касается валидации: кидать исключения с основного конструктора - не очень хорошая затея;
         * доменный объект сам отвечает за свою целостность и валидация здесь - последний рубеж валидации,
         * несмотря на то, что данные могут валидировать уровни выше (сервис, валидация DTO и тд). */
        public static Book Create(string title, int year)
        {
            ValidateParameters(title, year); // вынесем валидацию в отдельный метод

            return new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Year = year
            };
        }

        public void AddAuthor(Author author)
        {
            if(author is null)
                throw new InvalidBookAuthorException("The book author cannot be null.");

            if (_authors.Any(a => a.Id == author.Id))
                return;

            _authors.Add(author);
        }

        public void AddAuthorsRange(IEnumerable<Author> authors)
        {
            if (authors is null || !authors.Any())
                throw new InvalidBookAuthorException("The book authors collection cannot be null or empty.");

            foreach (var author in authors)
            {
                AddAuthor(author);
            }
        }

        public void ChangeAuthorsRange(IEnumerable<Author> authors)
        {
            if (authors is null || !authors.Any())
                throw new InvalidBookAuthorException("The book authors collection cannot be null or empty.");

            _authors.Clear();
            foreach (var author in authors)
            {
                AddAuthor(author);
            }
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
