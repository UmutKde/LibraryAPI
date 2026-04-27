using System.Security.Claims;
using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Enums;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation.HasPermissionAttribute;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> GetAllActiveLoan()
    {
        var loans = await _service.GetAllActiveLoanAsync();
        return Ok(loans);
    }
    [HttpGet]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> GetAllLoans()
    {
        var loans = await _service.GetAllLoansAsync();
        return Ok(loans);
    }

    //Kullanıcı herhangi bir loan için ekstran bilgi almak istediği için
    [HttpGet("{loanId:int}", Name = "GetLoanById")]
    [Authorize]
    public async Task<IActionResult> GetLoanById([FromRoute] int loanId)
    {
        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !int.TryParse(currentUserIdStr, out int currentUserId))
            return Unauthorized("Kimliğiniz doğrulanamadı");


        var isAdmin = User.IsInRole("Admin") || User.IsInRole("Editor");
        var loan = await _service.GetLoanByIdAsync(loanId);
        if (!isAdmin && currentUserId != loan.UserId)
            return Forbid();
        return Ok(loan);
    }

    [HttpGet("barcode/{barcode}", Name = "GetLoanByBarcode")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<IActionResult> GetLoanByBarcode([FromRoute] string barcode)
    {
        var loan = await _service.GetLoanByBarcode(barcode);
        return Ok(loan);
    }

    // Kullanıcı tüm loanları görsün diye
    [HttpGet("user/{id:int}", Name = "GetLoansByUserId")]
    [Authorize]
    public async Task<IActionResult> GetLoansByUserId([FromRoute] int id)
    {
        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !int.TryParse(currentUserIdStr, out int currentUserId))
            return Unauthorized("Kimliğiniz doğrulanamadı");

        var isAuthorizedRole = User.IsInRole("Admin") || User.IsInRole("Editor");

        if (!isAuthorizedRole && currentUserId != id)
            return Forbid();

        // BURASI DEĞİŞTİ: Artık URL'den gelen id'yi yolluyoruz
        var loans = await _service.GetLoansByUserIdAsync(id);
        return Ok(loans);
    }

    // Kullanıcı kendi aktif loanlarını görmesi için
    [HttpGet("user/{id:int}/active", Name = "GetActiveLoansByUserId")]
    [Authorize]
    public async Task<IActionResult> GetActiveLoansByUserId([FromRoute] int id)
    {
        var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserIdStr) || !int.TryParse(currentUserIdStr, out int currentUserId))
            return Unauthorized("Kimliğiniz doğrulanamadı");

        var isAuthorizedRole = User.IsInRole("Admin") || User.IsInRole("Editor");

        if (!isAuthorizedRole && currentUserId != id)
            return Forbid();

        // URL'den gelen id servise yollandı
        var loans = await _service.GetActiveLoansByUserIdAsync(id);
        return Ok(loans);
    }

    [HttpPost]
    [HasPermission<Loan>(ActionType.Create)]

    public async Task<IActionResult> CreateLoan([FromBody] LoanDtoForInsertion loanDtoForInsertion)
    {
        var createdLoan = await _service.CreateLoan(loanDtoForInsertion);
        return StatusCode(201, createdLoan);
    }

    [HttpPut("return")]
    [HasPermission<Loan>(ActionType.Update)]
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