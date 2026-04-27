using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Enums;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Entities;
using LibrarySystem.Presentation.HasPermissionAttribute;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _service;

    public PublisherController(IPublisherService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPublishersAsync()
    {
        var publishers = await _service.GetAllPublisherAsync();
        return Ok(publishers);
    }

    [HttpGet("{id:int}", Name = "GetPublisherById")]
    public async Task<IActionResult> GetPublisherById([FromRoute] int id)
    {
        var publisher = await _service.GetPublisherByIdAsync(id);
        return Ok(publisher);
    }
    [HttpPost]
    [HasPermission<Publisher>(ActionType.Create)]

    public async Task<IActionResult> CreatePublisher([FromBody] PublisherDtoForInsertion publisherDtoForInsertion)
    {
        var createdPublisher = await _service.CreatePublisherAsync(publisherDtoForInsertion);
        return CreatedAtRoute("GetPublisherById", new { id = createdPublisher.Id }, createdPublisher);
    }

    [HttpPut]
    [HasPermission<Publisher>(ActionType.Update)]
    public async Task<IActionResult> UpdatePublisher([FromBody] PublisherDtoForUpdate publisherDtoForUpdate)
    {
        await _service.UpdatePublisherAsync(publisherDtoForUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [HasPermission<Publisher>(ActionType.Delete)]
    public async Task<IActionResult> DeletePublisher([FromRoute] int id)
    {
        await _service.DeletePublisherAsync(id);
        return NoContent();
    }
}