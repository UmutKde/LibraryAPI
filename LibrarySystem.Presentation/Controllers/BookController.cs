using LibrarySystem.Application.Dtos.BookDtos;
using LibrarySystem.Application.Enums;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation.HasPermissionAttribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _service;

    public BookController(IBookService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _service.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id:int}", Name = "GetBookById")]
    public async Task<IActionResult> GetBookById([FromRoute] int id)
    {
        var book = await _service.GetBookByIdAsync(id);
        return Ok(book);
    }

    [HttpPost]
    [HasPermission<Book>(ActionType.Create)]
    public async Task<IActionResult> CreateBook([FromBody] BookDtoForInsertion bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdBook = await _service.CreateBookAsync(bookDto);
        return CreatedAtRoute("GetBookById", new { id = createdBook.Id }, createdBook);
    }

    [HttpPut]
    [HasPermission<Book>(ActionType.Update)]
    public async Task<IActionResult> UpdateBook([FromBody] BookDtoForUpdate bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        await _service.UpdateBookAsync(bookDto);
        return NoContent();

    }

    [HttpDelete("{id}")]
    [HasPermission<Book>(ActionType.Delete)]
    public async Task<IActionResult> DeleteBook([FromRoute] int id)
    {
        await _service.DeleteBookAsync(id);
        return NoContent();
    }
}