using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireGuideRole")]
public class GuideController(ISender sender) : BaseController(sender)
{
    /*[HttpDelete("delete/picture")]*/
    
}