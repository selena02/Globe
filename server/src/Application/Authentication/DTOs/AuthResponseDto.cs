namespace Application.Authentication.DTOs;

public record AuthResponseDto(
    string Token,
    int? Id,
    IList<string> Roles
);