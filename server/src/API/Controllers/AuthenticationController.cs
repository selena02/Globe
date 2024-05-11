using Application.Authentication.Commands.LoginUser;
using Application.Authentication.Commands.RegisterUser;
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
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return Ok(result);
    }
}