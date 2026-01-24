namespace LibrarySystem.Application.Dtos;

public class PublisherDtoForUpdate
{
    public int Id { get; set; }
    public string? PublisherName { get; set; }
    public string? ImageUrl { get; set; }
    public string? ContactEmail { get; set; }
    public string? PhoneNumber { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? ContactPerson { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
}