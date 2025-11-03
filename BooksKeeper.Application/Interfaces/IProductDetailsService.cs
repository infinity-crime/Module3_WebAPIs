using BooksKeeper.Application.Common;
using BooksKeeper.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces
{
    /// <summary>
    /// Сервис для получения подробной информации о продукте, включая детали книги и отзывы.
    /// </summary>
    public interface IProductDetailsService
    {
        /// <summary>
        /// Получение подробной информации о книге по ее ID, включая детали книги и отзывы.
        /// </summary>
        /// <param name="id">Id книги (Guid)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns><see cref="ProductDetailsResponse"/>, который содержит Dto книги с авторами и список отзывов</returns>
        Task<Result<ProductDetailsResponse>> GetBookDetailsAsync(Guid id, CancellationToken cancellationToken);
    }
}
