using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Data;
using LibraryApi.DTOs.Loan;
using LibraryApi.DTOs.User;
using LibraryApi.Entites;
using LibraryApi.Services;
using LibraryApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet("user/{id}"), Authorize]
        public async Task<ActionResult<IEnumerable<GetLoanDTO>>> GetLoans(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != Int32.Parse(userId))
            {
                throw new UnauthorizedAccessException();
            }
            var loans = await _loanService.GetLoansByUserId(id);
            if (loans == null)
            {
                return NotFound();
            }
            return Ok(loans);
        }

        [HttpGet("{id}"), Authorize]
        public async Task<ActionResult<GetLoanDTO>> GetLoan(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loan = await _loanService.GetLoanById(id, Int32.Parse(userId));
            return Ok(loan);
        }

        [HttpGet, Authorize(Roles = "Admin,Librarian")]
        public async Task<ActionResult<IEnumerable<GetLoanDTO>>> GetAllLoans()
        {
            var loans = await _loanService.GetAllLoans();
            if (loans == null)
            {
                return NotFound();
            }
            return Ok(loans);
        }

        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateLoan(CreateLoanDTO request)
        {
            var result = await _loanService.CreateLoan(request);
            return CreatedAtAction("GetLoan", new { id = result.Id }, null);
        }

        [HttpPost("end"), Authorize]
        public async Task<IActionResult> EndLoan(FinishLoanDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _loanService.FinishLoan(request, Int32.Parse(userId));
            return Ok(result.Id);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _loanService.DeleteLoan(id, Int32.Parse(userId));
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}