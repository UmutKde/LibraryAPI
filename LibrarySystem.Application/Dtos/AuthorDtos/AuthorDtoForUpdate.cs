namespace LibrarySystem.Application.Dtos;

public class AuthorDtoForUpdate
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? ImageUrl { get; set; }
    public string? Summary { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Country { get; set; }
    public string? WebSiteUrl { get; set; }
}