using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Services;
using BooksKeeper.Domain.Entities;
using BooksKeeper.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;

namespace BooksKeeper.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public async Task GetByIdAsync_BookExists_ReturnsBook()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            // Подготовка мок-объекта для метода GetByIdAsync
            var expectedBook = Book.Create("Test Book", 2023);

            // Настройка мока для возврата ожидаемой книги
            bookRepositoryMock
                .Setup(r => r.GetByIdAsync(expectedBook.Id, It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(expectedBook);

            // Настройка мока для возврата null из кэша
            cacheServiceMock
                .Setup(c => c.GetAsync<BookResponse>(It.IsAny<string>()))
                .ReturnsAsync((BookResponse?)null);

            // Создание экземпляра BookService с использованием мок-объекта
            var service = new BookService(bookRepositoryMock.Object, null!, null!, null!, cacheServiceMock.Object);

            // Act
            var result = await service.GetByIdAsync(expectedBook.Id);

            // Assert - проверка результата
            Assert.NotNull(result.Value);
            Assert.Equal(expectedBook.Id, result.Value.Id);
            Assert.Equal(expectedBook.Title, result.Value.Title);
        }
    }
}
