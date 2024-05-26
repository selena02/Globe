using Application.Pilot.Commands.DeleteUser;
using Application.Pilot.Commands.UpdateRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequirePilotRole")]
public class PilotController(ISender sender) : BaseController(sender)
{
    [HttpPut("update/role/{id}")]
    public async Task<IActionResult> UpdateRole(int id, CancellationToken cancellationToken)
    {
        var command = new UpdateRoleCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
}