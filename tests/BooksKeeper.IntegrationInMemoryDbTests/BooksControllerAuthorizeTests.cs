using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.IntegrationInMemoryDbTests
{
    public class BooksControllerAuthorizeTests
    {
        [Fact]
        public async Task GetUserInfo_WithoutToken_ReturnsUnauthorizedAsync()
        {
            // Arrange
            var factory = new MyTestFactory();
            var client = factory.CreateClient();

            // Act
            var respone = await client.GetAsync("api/books/my-info");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, respone.StatusCode);
        }
    }
}
