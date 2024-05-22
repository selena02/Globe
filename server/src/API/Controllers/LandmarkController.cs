using Application.Landmarks.Commands.ClassifyLandmark;
using Application.Landmarks.Commands.DeleteLandmark;
using Application.Landmarks.Commands.SaveLandmark;
using Application.Landmarks.Queries.GetAllCoordinates;
using Application.Landmarks.Queries.GetLandmarkById;
using Application.Landmarks.Queries.GetUserLandmarks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LandmarkController(ISender sender) : BaseController(sender)
{
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserLandmarks(int id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetUserLandmarksQuery(id), cancellationToken);

        return Ok(result);
    }
    
    
    [HttpGet("coordinates")]
    public async Task<IActionResult> GetAllCoordinates(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllCoordinatesQuery(), cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLandmarkById(int id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetLandmarkByIdQuery(id), cancellationToken);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> ClassifyLandmark([FromForm] ClassifyLandmarkCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("save")]
    public async Task<IActionResult> SaveLandmark(SaveLandmarkCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLandmark(int id, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new DeleteLandmarkCommand(id), cancellationToken);

        return Ok(result);
    }
}