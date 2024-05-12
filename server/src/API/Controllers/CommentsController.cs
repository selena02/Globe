using Application.Comments.Commands.UploadComment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CommentsController(ISender sender) : BaseController(sender)
{
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadComment(UploadCommentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
}