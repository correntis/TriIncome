using Microsoft.AspNetCore.Mvc;
using LoanApi.Models;
using LoanApi.Services;

namespace LoanApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Loan>>> GetLoans(
        [FromQuery] LoanStatus? status = null,
        [FromQuery] decimal? minAmount = null,
        [FromQuery] decimal? maxAmount = null,
        [FromQuery] int? minTerm = null,
        [FromQuery] int? maxTerm = null)
    {
        var loans = await _loanService.GetLoansAsync(status, minAmount, maxAmount, minTerm, maxTerm);
        return Ok(loans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Loan>> GetLoan(int id)
    {
        var loan = await _loanService.GetLoanByIdAsync(id);
        return Ok(loan);
    }

    [HttpPost]
    public async Task<ActionResult<Loan>> CreateLoan([FromBody] CreateLoanDto createLoanDto)
    {
        var loan = await _loanService.CreateLoanAsync(createLoanDto);
        return CreatedAtAction(nameof(GetLoan), new { id = loan.Id }, loan);
    }

    [HttpPatch("{id}/toggle-status")]
    public async Task<ActionResult<Loan>> ToggleLoanStatus(int id)
    {
        var loan = await _loanService.ToggleLoanStatusAsync(id);
        return Ok(loan);
    }
}
