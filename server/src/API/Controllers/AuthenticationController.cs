using Application.Authentication.Commands.RegisterUser;
using Application.Authentication.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _sender;

    public AuthenticationController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(registerDto);

        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}