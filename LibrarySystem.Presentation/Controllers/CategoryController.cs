using LibrarySystem.Application.Dtos.CategoryDtos;
using LibrarySystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _service.GetAllCategoryAsync();
        return Ok(categories);
    }

    [HttpGet("{id:int}", Name = "GetCategoryById")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        var category = await _service.GetOneCategoryAsync(id);
        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDtoForInsertion categoryDtoForInsertion)
    {
        var createdCategory = await _service.CreateCategoryAsync(categoryDtoForInsertion);
        return CreatedAtRoute("GetCategoryById", new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut]
    public async Task<IActionResult> Updatecategory([FromBody] CategoryDtoForUpdate categoryDtoForUpdate)
    {
        await _service.UpdateCategoryAsync(categoryDtoForUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        await _service.DeleteCategoryAsync(id);
        return NoContent();
    }
}