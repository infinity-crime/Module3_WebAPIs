using BooksKeeper.Domain.Common;
using BooksKeeper.Domain.Exceptions.AuthorExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Entities
{
    public class Author : BaseEntity<Guid>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();

        private Author() { }

        public static Author Create(string firstName, string lastName)
        {
            ValidateParameters(firstName, lastName);

            return new Author
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName
            };
        }

        public void ChangeFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidAuthorFirstNameException("The author first name cannot be empty.");

            FirstName = firstName;
        }

        public void ChangeLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new InvalidAuthorLastNameException("The author last name cannot be empty.");

            LastName = lastName;
        }

        private static void ValidateParameters(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidAuthorFirstNameException("The author first name cannot be empty.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new InvalidAuthorLastNameException("The author last name cannot be empty.");
        }
    }
}
