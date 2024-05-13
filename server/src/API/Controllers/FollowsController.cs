using Application.Follows.Commands.FollowUser;
using Application.Follows.Commands.RemoveNotification;
using Application.Follows.Commands.UnfollowUser;
using Application.Follows.Queries.GetFollowStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class FollowsController(ISender sender) : BaseController(sender)
{
    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetFollowStatus(int id, CancellationToken cancellationToken)
    {
        var query = new GetFollowStatusQuery(id);
        
        var response = await Sender.Send(query, cancellationToken);
        
        return Ok(response);
    }
    
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
    
    [HttpDelete("notification/{id}")]
    public async Task<IActionResult> RemoveNotification(int id, CancellationToken cancellationToken)
    {
        var command = new RemoveNotificationCommand(id);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return Ok(response);
    }
}