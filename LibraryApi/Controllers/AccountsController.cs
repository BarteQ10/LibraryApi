using LibraryApi.Data;
using LibraryApi.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    public class AccountsController
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AccountController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public AccountController(ApplicationDbContext context)
            {
                _context = context;
            }

            
        }
    }
}
