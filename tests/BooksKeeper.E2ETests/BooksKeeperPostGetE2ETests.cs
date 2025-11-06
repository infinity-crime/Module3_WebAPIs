using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Domain.Entities;
using BooksKeeper.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BooksKeeper.E2ETests
{
    public class BooksKeeperPostGetE2ETests
    {
        [Fact]
        public async Task PostAndGet_Book_E2E_WithPostgresContainer()
        {
            using var factory = new PostgresTestFactory();
            using var client = factory.CreateClient();

            Guid seedAuthorId;
            using (var scope = factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var author = Author.Create("Mikle", "Jordan");
                db.Authors.Add(author);
                await db.SaveChangesAsync();
                seedAuthorId = author.Id;
            }

            // POST
            var createAuthorRequest = new CreateAuthorRequest("Created", "Author");

            var postResponse = await client.PostAsJsonAsync("/api/authors", createAuthorRequest);
            postResponse.EnsureSuccessStatusCode();

            // GET
            var getResponse = await client.GetAsync($"/api/authors/{seedAuthorId}");
            getResponse.EnsureSuccessStatusCode();

            var fetched = await getResponse.Content.ReadFromJsonAsync<TestResult>();
            Assert.Equal("Mikle", fetched?.Value.FirstName);
        }
    }

    public class TestResult
    {
        public bool IsSuccess { get; set; }
        public AuthorResponse Value { get; set; }
    }
}
