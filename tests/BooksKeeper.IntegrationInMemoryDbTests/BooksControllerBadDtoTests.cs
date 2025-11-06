using BooksKeeper.Application.DTOs.Requests.BookRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.IntegrationInMemoryDbTests
{
    public class BooksControllerBadDtoTests
    {
        [Fact]
        public async Task CreateBook_WithBadDto_ReturnsBadRequestAsync()
        {
            // Arrange
            var factory = new MyTestFactory();
            var client = factory.CreateClient();

            var badCreateRequest = new CreateBookRequest(string.Empty, 132, new List<Guid> { });

            // Act
            var response = await client.PostAsJsonAsync("/api/books", badCreateRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
