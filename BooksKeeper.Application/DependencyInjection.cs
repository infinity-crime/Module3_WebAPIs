using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksKeeper.Application.Authorization.Handlers;
using BooksKeeper.Application.Authorization.Requirements;
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
using Polly.Extensions.Http;

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

            services.AddHttpClient<IHttpAuthorService, HttpAuthorService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7120");
            })
            .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(1)))
            .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            return services;
        }
    }
}
