using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Requests.AuthorRequests;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Interfaces.Common;
using BooksKeeper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Services
{
    public class HttpAuthorService : IHttpAuthorService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpAuthorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<AuthorDto?> CreateAsync(CreateAuthorRequest request)
        {
            using var res = await _httpClient.PostAsJsonAsync("/api/authors", request);
            if(res.IsSuccessStatusCode)
            {
                var successResult = await res.Content.ReadFromJsonAsync<SuccessResultDto<AuthorDto>>(_jsonSerializerOptions);

                return successResult!.Value;
            }
            
            return null;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using var res = await _httpClient.DeleteAsync($"/api/authors/delete/{id}");

            return res.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAsync()
        {
            using var res = await _httpClient.GetAsync("/api/authors/all-authors");
            
            return await res.Content.ReadFromJsonAsync<IEnumerable<AuthorResponse>>(_jsonSerializerOptions) 
                ?? Enumerable.Empty<AuthorResponse>();
        }

        public async Task<AuthorResponse?> GetByIdAsync(Guid id)
        {
            using var res = await _httpClient.GetAsync($"/api/authors/{id}");
            if(res.IsSuccessStatusCode)
            {
                var successResult = await res.Content.ReadFromJsonAsync<SuccessResultDto<AuthorResponse>>(_jsonSerializerOptions);

                return successResult!.Value;
            }

            return null;
        }

        public async Task<IEnumerable<AuthorDto>> GetByIdRangeAsync(List<Guid> ids)
        {
            var query = string.Join("&", ids.Select(i => $"Ids={i}"));

            var res = await _httpClient.GetAsync($"/api/authors/range?{query}");
            if(res.IsSuccessStatusCode)
            {
                var succesResult = await res.Content.ReadFromJsonAsync<SuccessResultDto<IEnumerable<AuthorDto>>>(_jsonSerializerOptions);

                return succesResult!.Value;
            }

            return Enumerable.Empty<AuthorDto>();
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateAuthorRequest request)
        {
            using var res = await _httpClient.PutAsJsonAsync($"/api/authors/update/{id}", request);

            return res.IsSuccessStatusCode;
        }

        private class SuccessResultDto<T>
        {
            public T Value { get; set; }
            public bool IsSuccess { get; set; }
        }
    }
}
