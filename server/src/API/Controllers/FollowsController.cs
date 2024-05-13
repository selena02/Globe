using Application.Follows.Commands.FollowUser;
using Application.Follows.Commands.UnfollowUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FollowsController(ISender sender) : BaseController(sender)
{
    [HttpPost("{id}")]
    public async Task<IActionResult> FollowUser(int id, CancellationToken cancellationToken)
    {
        var command = new FollowUserCommand(id);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> UnfollowUser(int id, CancellationToken cancellationToken)
    {
        var command = new UnfollowUserCommand(id);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return Ok(response);
    }
}