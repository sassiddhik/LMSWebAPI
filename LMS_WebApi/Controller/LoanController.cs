using AutoMapper;
using LMS_WebApi.Interface;
using LMS_WebApi.Models;
using LMS_WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LMS_WebApi.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;

namespace LMS_WebApi.Controller
{
    [ApiController]
    [Route("api/v1/loan")]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService<Loan> _loanService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public LoanController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            //_mapper = mapper;
        }

        // GET: api/loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            return await _dbContext.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .ToListAsync();
        }

        // POST: api/loans/borrow
        [HttpPost("borrow")]
        public async Task<ActionResult<Loan>> BorrowBook(int memberId, int bookId)
        {
            var member = await _dbContext.Users.FindAsync(memberId);
            var book = await _dbContext.Books.FindAsync(bookId);

            if (member == null || book == null)
                return NotFound("Member or Book not found.");

            if (!book.IsAvailable)
                return BadRequest("Book is already borrowed.");

            // Create loan 
            var loan = new Loan
            {
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.Now,
                IsReturned = false
            };

            book.IsAvailable = false; // mark as borrowed

            _dbContext.Loans.Add(loan);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoans), new { id = loan.LoanId }, loan);
        }

        // POST: api/loans/return
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(int loanId)
        {
            var loan = await _dbContext.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

            if (loan == null)
                return NotFound("Loan record not found.");

            if (loan.IsReturned)
                return BadRequest("Book already returned.");

            // Update records
            loan.IsReturned = true;
            loan.ReturnDate = DateTime.Now;

            loan.Book.IsAvailable = true;

            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Book returned successfully.",
                LoanId = loan.LoanId,
                ReturnDate = loan.ReturnDate
            });
        }
    }
}

