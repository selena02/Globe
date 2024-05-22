namespace Application.Common.Models;

public record UserDto(
    int UserId,
    string Username,
    string? ProfilePicture,
    DateTime CreatedAt,
    bool IsGuide);
    