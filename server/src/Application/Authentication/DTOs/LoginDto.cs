namespace Application.Authentication.DTOs;

public record LoginDto(
    string? Email,
    string? Password
);
