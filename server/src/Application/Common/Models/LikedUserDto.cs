namespace Application.Common.Models;

public class LikedUserDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string? ProfilePictureUrl { get; set; }
}