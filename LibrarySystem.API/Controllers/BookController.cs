using LibrarySystem.Application.Dtos.BookDto;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers;

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
    public async Task<IActionResult> CreateBook([FromBody] BookDtoForInsertion bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var createdBook = await _service.CreateBookAsync(bookDto);
        return CreatedAtRoute("GetBookById", new { id = createdBook.Id }, createdBook);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] BookDtoForUpdate bookDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

            await _service.UpdateBookAsync(bookDto);
            return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook([FromRoute] int id)
    {
            await _service.DeleteBookAsync(id);
            return NoContent();
    }
}