namespace Application.Authentication.DTOs;

public record RegisterDto(
    string UserName,
    string Email,
    string Password,
    string FullName
);