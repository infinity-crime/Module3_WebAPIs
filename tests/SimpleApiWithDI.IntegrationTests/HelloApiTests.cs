using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SimpleApiWithDI.IntegrationTests
{
    public class HelloApiTests
    {
        [Fact]
        public async Task Get_HelloEndpoint_ReturnsOk()
        {
            // Arrange
            using var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("api/hello");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
