using LibrarySystem.Application.Dtos;
using LibrarySystem.Application.Interfaces;
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

    [HttpGet("{id:int}",Name = "GetPublisherById")]
    public async Task<IActionResult> GetPublisherById([FromRoute] int id)
    {
        var publisher = await _service.GetPublisherByIdAsync(id);
        return Ok(publisher);
    }    
    [HttpPost]
    public async Task<IActionResult> CreatePublisher([FromBody] PublisherDtoForInsertion publisherDtoForInsertion)
    {
        var createdPublisher = await _service.CreatePublisherAsync(publisherDtoForInsertion);
        return CreatedAtRoute("GetPublisherById",new {id = createdPublisher.Id},createdPublisher);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePublisher([FromBody] PublisherDtoForUpdate publisherDtoForUpdate)
    {
        await _service.UpdatePublisherAsync(publisherDtoForUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublisher([FromRoute] int id)
    {
        await _service.DeletePublisherAsync(id);
        return NoContent();
    }
}