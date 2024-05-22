using Application.Guide.Commands;
using Application.Guide.Commands.DeleteBio;
using Application.Guide.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireGuideRole")]
public class GuideController(ISender sender) : BaseController(sender)
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery();
        
        var result = await Sender.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("delete/picture/{id}")]
    public async Task<IActionResult> DeletePicture(int id, CancellationToken cancellationToken)
    {
        var command = new DeletePictureCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("delete/bio/{id}")]
    public async Task<IActionResult> DeleteBio(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteBioCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
}