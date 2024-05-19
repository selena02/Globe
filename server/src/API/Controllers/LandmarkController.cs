using Application.Landmarks.Commands.ClassifyLandmark;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LandmarkController(ISender sender) : BaseController(sender)
{
    [HttpPost]
    public async Task<IActionResult> ClassifyLandmark([FromForm] ClassifyLandmarkCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
}