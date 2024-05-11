namespace Application.Users.DTOs;

public record class EditProfileResponseDto
(
    string? Location,
    string? Bio,
    string? ProfilePictureUrl
);