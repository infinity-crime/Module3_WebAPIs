using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Infrastructure.Data;
using BooksKeeper.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace BooksKeeper.IntegrationInMemoryDbTests
{
    public class AuthorControllerTests
    {
        [Fact]
        public async Task GetAllAuthors_ReturnsAuthors()
        {
            // Arrange
            var factory = new MyTestFactory();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("api/authors/all-authors");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
