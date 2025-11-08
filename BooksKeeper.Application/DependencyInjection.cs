using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BooksKeeper.Application.Authorization.Handlers;
using BooksKeeper.Application.Authorization.Requirements;
using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs;
using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Interfaces.Identity;
using BooksKeeper.Application.Services;
using BooksKeeper.Application.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace BooksKeeper.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Регистрация сервисов приложения
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IProductReviewService, ProductReviewService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IProductDetailsService, ProductDetailsService>();

            // Регистрация сервисов идентификации
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = false,
                        
                        ValidIssuer = configuration["JwtOptions:Issuer"],
                        ValidAudience = configuration["JwtOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"]!))
                    };
                });

            // Регистрация политик авторизации
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OlderThan18", policy => policy.Requirements.Add(new AgeRequirement(18)));
            });

            // Регистрация обработчиков требований авторизации
            services.AddSingleton<IAuthorizationHandler, AgeHandler>();

            // Создание политик Polly
            var retryPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(msg => ((int)msg.StatusCode) >= 500 || msg.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(1));

            var circuitBreakerPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(r => ((int)r.StatusCode) >= 500 || r.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .Or<TimeoutRejectedException>()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3), TimeoutStrategy.Optimistic);

            var fallbackPolicy = Policy<HttpResponseMessage>
                .Handle<BrokenCircuitException<HttpResponseMessage>>()
                .Or<BrokenCircuitException>()
                .Or<TimeoutRejectedException>()
                .OrResult(msg => ((int)msg.StatusCode) >= 500 || msg.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .FallbackAsync(
                    fallbackAction: async (ct) =>
                    {
                        var fallbackAuthor = new AuthorResponse(Guid.Empty, "Unknown", "Unknown", new List<BookDto> { });

                        var fallbackResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                        {
                            Content = new StringContent(JsonSerializer.Serialize(new { Value = fallbackAuthor, IsSuccess = true }), Encoding.UTF8, "application/json")
                        };

                        return await Task.FromResult(fallbackResponse);
                    }
                );

            var policyWrap = Policy.WrapAsync(fallbackPolicy, circuitBreakerPolicy, retryPolicy, timeoutPolicy);

            services.AddHttpClient<IHttpAuthorService, HttpAuthorService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7120");
                client.Timeout = Timeout.InfiniteTimeSpan;
            })
            .AddPolicyHandler(policyWrap);

            return services;
        }
    }
}
