using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireTravellerRole")]
    [ApiController]
    public abstract class BaseAuthApiController : ControllerBase
    {
        protected readonly ISender Sender;

        protected BaseAuthApiController(ISender sender)
        {
            Sender = sender;
        }
    }
}