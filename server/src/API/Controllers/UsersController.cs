using Microsoft.AspNetCore.Mvc;
using Application.Users.Commands.EditUserProfile;
using Application.Users.DTOs;
using Application.Users.Queries.GetUserById;
using MediatR;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ISender sender) : BaseController(sender)
{
    [HttpPut("edit")]
    public async Task<IActionResult> EditUserProfile([FromForm]EditProfileDto editProfileDto, CancellationToken cancellationToken)
    {
        var command = new EditUserProfileCommand(editProfileDto);

        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return Ok(result);
    }
}