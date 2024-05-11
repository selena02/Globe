using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireTravellerRole")]
    [ApiController]
    public abstract class BaseController(ISender sender) : ControllerBase
    {
        protected readonly ISender Sender = sender;
    }
}