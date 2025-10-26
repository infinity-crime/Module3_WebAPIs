using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Common
{
    public class Result
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Error? Error { get; }
        public bool IsSuccess { get; }

        protected Result()
        {
            IsSuccess = true;
            Error = default;
        }

        protected Result(Error? error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static Result Success() => new();
        public static Result Failure(Error error) => new(error);
    }

    public sealed class Result<T> : Result
    {
        private readonly T? _value;

        public T Value => IsSuccess ? _value!
            : throw new InvalidOperationException("The value cannot be retrieved if the IsSuccess " +
                "flag is negative.");

        private Result(T value) : base() => _value = value;
        private Result(Error error) : base(error) => _value = default;

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(Error error) => new(error);
    }
}
