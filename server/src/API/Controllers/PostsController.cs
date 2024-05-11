using Application.Posts.Commands.UploadPost;
using Application.Posts.Queries.GetPostById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PostsController(ISender sender) : BaseController(sender)
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadPost([FromForm]UploadPostCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id, CancellationToken cancellationToken)
    {
        var query = new GetPostByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return Ok(result);
    }
}