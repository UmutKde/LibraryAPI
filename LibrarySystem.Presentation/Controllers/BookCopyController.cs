using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Enums;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation.HasPermissionAttribute;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[Route("api/book-copies")]
[ApiController]
public class BookCopyController : ControllerBase
{
    private readonly IBookCopyService _service;

    public BookCopyController(IBookCopyService service)
    {
        _service = service;
    }

    [HttpGet("by-book/{bookId}")]
    public async Task<IActionResult> GetAllByBookId(int bookId)
    {
        var copies = await _service.GetAllCopiesByBookIdAsync(bookId);
        return Ok(copies);
    }

    [HttpPost]
    [HasPermission<BookCopy>(ActionType.Create)]
    public async Task<IActionResult> CreateCopies([FromBody] BookCopyDtoForInsertion bookCopyDtoForInsertion)
    {
        var barcodes = await _service.CreateCopiesAsync(bookCopyDtoForInsertion);
        return Ok(barcodes);
    }

    [HttpDelete("{id}")]
    [HasPermission<BookCopy>(ActionType.Delete)]
    public async Task<IActionResult> DeleteCopy(int id)
    {
        await _service.DeleteCopyAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    [HasPermission<BookCopy>(ActionType.Update)]
    public async Task<IActionResult> UpdateCopy(int id,[FromBody] BookCopyDtoForUpdate bookCopyDtoForUpdate)
    {
        if (id != bookCopyDtoForUpdate.Id)
            return BadRequest("URL ID and Body ID mismatch.");
            
        await _service.UpdateCopyAsync(bookCopyDtoForUpdate);
        return NoContent();
    }
}