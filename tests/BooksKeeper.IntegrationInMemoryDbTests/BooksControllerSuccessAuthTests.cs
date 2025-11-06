using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.IntegrationInMemoryDbTests
{
    public class BooksControllerSuccessAuthTests
    {
        [Fact]
        public async Task GetAllBooks_WithToken_ReturnsOk()
        {
            // Arrange
            var factory = new MyTestFactory();
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/books/all-books");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
