using Application.Guide.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireGuideRole")]
public class GuideController(ISender sender) : BaseController(sender)
{
    [HttpDelete("delete/picture/{id}")]
    public async Task<IActionResult> DeletePicture(int id, CancellationToken cancellationToken)
    {
        var command = new DeletePictureCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
}