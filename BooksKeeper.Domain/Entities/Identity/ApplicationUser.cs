using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksKeeper.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateOnly DateOfBirth { get; set; }
    }
}
