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
using BooksKeeper.Domain.Interfaces.Common;
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

        [Fact]
        public async Task DeleteAsync_BookExists_CallDeleteEntityAsync()
        {
            // Arrange
            var bookRepositoryMock = new Mock<IBookRepository>();
            var cacheServiceMock = new Mock<ICacheService>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var expectedBook = Book.Create("Test Book", 2023);

            bookRepositoryMock
                .Setup(r => r.GetByIdAsync(expectedBook.Id, true, true))
                .ReturnsAsync(expectedBook);

            // Настройка мока для метода Delete не требуется, так как он ничего не возвращает

            unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            cacheServiceMock
                .Setup(c => c.RemoveAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var service = new BookService(bookRepositoryMock.Object, null!, unitOfWorkMock.Object, null!, cacheServiceMock.Object);

            // Act
            var result = await service.DeleteByIdAsync(expectedBook.Id);

            // Assert
            Assert.True(result.IsSuccess);

            // Verify вызов метода Delete с правильным объектом Book и SaveChangesAsync
            bookRepositoryMock.Verify(r => r.Delete(It.Is<Book>(b => b.Id == expectedBook.Id)), Times.Once);
            unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
