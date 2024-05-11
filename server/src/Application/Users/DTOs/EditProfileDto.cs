using Microsoft.AspNetCore.Http;

namespace Application.Users.DTOs;

public class EditProfileDto
{
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}