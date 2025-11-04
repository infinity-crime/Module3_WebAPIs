using BooksKeeper.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Application.Interfaces.Identity
{
    public interface IJwtService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
