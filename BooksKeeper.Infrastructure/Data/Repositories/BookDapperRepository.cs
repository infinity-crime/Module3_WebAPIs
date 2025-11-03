using BooksKeeper.Application.DTOs.Responses;
using BooksKeeper.Domain.Interfaces;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Infrastructure.Data.Repositories
{
    public class BookDapperRepository : IBookDapperRepository<BookYearCountResponse>
    {
        private readonly ApplicationDbContext _context;
        public BookDapperRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookYearCountResponse>> GetBooksByYearCountAsync()
        {
            using var connection = _context.Database.GetDbConnection();
            const string sql = @"
                SELECT ""Year"", COUNT(*) AS ""Count""
                FROM ""Books""
                GROUP BY ""Year""
                ORDER BY ""Year""";

            return await connection.QueryAsync<BookYearCountResponse>(sql);
        }
    }
}
