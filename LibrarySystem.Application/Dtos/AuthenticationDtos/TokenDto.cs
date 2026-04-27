namespace LibrarySystem.Application.Dtos;

public record TokenDto
(
    string AccessToken,
    string RefreshToken
);