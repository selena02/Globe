namespace Application.Users.DTOs;
public record UserByIdResponse(
    int Id,
    string UserName,
    string FullName,
    string Email,
    string? ProfilePictureUrl,
    string? Location,
    string? Bio,
    int FollowersCount,
    int FollowingCount
);