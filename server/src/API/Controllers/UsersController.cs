using Microsoft.AspNetCore.Mvc;
using Application.Users.Commands.EditUserProfile;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetUserFollowers;
using Application.Users.Queries.GetUsersSearchBar;
using MediatR;

namespace API.Controllers;
public class UsersController(ISender sender) : BaseController(sender)
{
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
    
    [HttpGet("{id}/followers")]
    public async Task<IActionResult> GetUserFollowers(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserFollowersQuery(id);
        
        var result = await Sender.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("{id}/following")]
    public async Task<IActionResult> GetUserFollowing(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserFollowingQuery(id);
        
        var result = await Sender.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPut("edit")]
    public async Task<IActionResult> EditUserProfile([FromForm]EditUserProfileCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
}