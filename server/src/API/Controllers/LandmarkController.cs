﻿using Application.Landmarks.Commands.ClassifyLandmark;
using Application.Landmarks.Commands.SaveLandmark;
using Application.Landmarks.Queries.GetAllCoordinates;
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
    
    [HttpPost("save")]
    public async Task<IActionResult> SaveLandmark(SaveLandmarkCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("coordinates")]
    public async Task<IActionResult> GetAllCoordinates(CancellationToken cancellationToken)
    {
        var result = await Sender.Send(new GetAllCoordinates(), cancellationToken);

        return Ok(result);
    }
}