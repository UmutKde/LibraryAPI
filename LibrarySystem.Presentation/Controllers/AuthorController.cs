using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Enums;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation.HasPermissionAttribute;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _service;

    public AuthorController(IAuthorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _service.GetAllAuthorsAsync();
        return Ok(authors);
    }

    [HttpGet("{id:int}", Name = "GetAuthorById")]
    public async Task<IActionResult> GetAuthorById([FromRoute] int id)
    {
        var author = await _service.GetAuthorByIdAsync(id);
        return Ok(author);
    }

    [HttpPost]
    [HasPermission<Author>(ActionType.Create)]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorDtoForInsertion authorDtoForInsertion)
    {
        var createdAuthor = await _service.CreateAuthorAsync(authorDtoForInsertion);
        return CreatedAtRoute("GetAuthorById", new { id = createdAuthor.Id }, createdAuthor);
    }

    [HttpPut]
    [HasPermission<Author>(ActionType.Update)]
    public async Task<IActionResult> UpdateAuthor([FromBody] AuthorDtoForUpdate authorDtoForUpdate)
    {
        await _service.UpdateAuthorAsync(authorDtoForUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission<Author>(ActionType.Delete)]
    public async Task<IActionResult> DeleteAuthor([FromRoute] int id)
    {
        await _service.DeleteAuthorAsync(id);
        return NoContent();
    }
}
