using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksKeeper.Application.Interfaces;
using BooksKeeper.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BooksKeeper.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IProductReviewService, ProductReviewService>();
            services.AddScoped<IProductDetailsService, ProductDetailsService>();

            return services;
        }
    }
}
