using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Exceptions.Common
{
    public class DomainException : Exception
    {
        public string Code { get; }
        public DomainException(string code, string message) : base(message)
        {
            Code = code;
        }
    }

    /* 
     * Создание своих типов исключений позволит отделить доменные ошибки от других
     * (например системных). А также даст возможность обрабатывать их на уровне 
     * приложения соответ. образом.
     */
}
