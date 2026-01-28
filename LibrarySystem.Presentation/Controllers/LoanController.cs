using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace LibrarySystem.Presentation.Controllers;

[Route("api/loans")]
[ApiController]
public class LoanController : ControllerBase
{
    private readonly ILoanService _service;

    public LoanController(ILoanService service)
    {
        _service = service;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetAllActiveLoan()
    {
        var loans = await _service.GetAllActiveLoanAsync();
        return Ok(loans);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllLoans()
    {
        var loans = await _service.GetAllLoansAsync();
        return Ok(loans);
    }

    [HttpGet("{id:int}", Name = "GetLoanById")]
    public async Task<IActionResult> GetLoanById([FromRoute] int id)
    {
        var loan = await _service.GetLoanByIdAsync(id);
        return Ok(loan);
    }

    [HttpGet("barcode/{barcode}", Name = "GetLoanByBarcode")]
    public async Task<IActionResult> GetLoanByBarcode([FromRoute] string barcode)
    {
        var loan = await _service.GetLoanByBarcode(barcode);
        return Ok(loan);
    }

    [HttpGet("user/{id:int}", Name = "GetLoansByUserId")]
    public async Task<IActionResult> GetLoansByUserId([FromRoute] int id)
    {
        var loans = await _service.GetLoansByUserIdAsync(id);
        return Ok(loans);
    }

    [HttpGet("user/{id:int}/active", Name = "GetActiveLoansByUserId")]
    public async Task<IActionResult> GetActiveLoansByUserId([FromRoute] int id)
    {
        var loans = await _service.GetActiveLoansByUserIdAsync(id);
        return Ok(loans);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoan([FromBody] LoanDtoForInsertion loanDtoForInsertion)
    {
        var createdLoan = await _service.CreateLoan(loanDtoForInsertion);
        return StatusCode(201, createdLoan);
    }

    [HttpPut("return")]
    public async Task<IActionResult> ReturnLoan([FromBody] LoanDtoForUpdate loanDtoForUpdate)
    {
        var result = await _service.UpdateLoan(loanDtoForUpdate);

        return Ok(new
        {
            Penalty = result.totalCost,
            Message = result.message
        });
    }

}