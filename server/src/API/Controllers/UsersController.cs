using Microsoft.AspNetCore.Mvc;
using Application.Users.Commands.EditUserProfile;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetUsersSearchBar;
using MediatR;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ISender sender) : BaseController(sender)
{
    [HttpPut("edit")]
    public async Task<IActionResult> EditUserProfile([FromForm]EditUserProfileCommand command, CancellationToken cancellationToken)
    {
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
    
    [HttpGet("search-bar")]
    public async Task<IActionResult> GetUsersSearchBar(CancellationToken cancellationToken)
    {
        var query = new GetUsersSearchBarQuery();

        var result = await Sender.Send(query, cancellationToken);

        return Ok(result);
    }
}