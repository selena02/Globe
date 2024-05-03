namespace Application.Authentication.DTOs;

public record AuthResponseDto(
    string? Token,
    string? Id,
    List<string>? Roles
);