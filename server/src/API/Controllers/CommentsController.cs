using API.Helpers;
using Application.Comments.Commands.DeleteComment;
using Application.Comments.Commands.UploadComment;
using Application.Comments.Queries.GetCommentLikes;
using Application.Comments.Queries.GetPostComments;
using Application.Common.Models;
using Application.Common.Utils;
using Application.Likes.Commands.LikeComment;
using Application.Likes.Commands.UnlikeComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CommentsController(ISender sender) : BaseController(sender)
{
    [AllowAnonymous]
    [HttpGet("post/{id}")]
    public async Task<IActionResult> GetPostComments([FromQuery]PaginationParameters paginationParameters, int id, CancellationToken cancellationToken)
    {
        var query = new GetPostCommentsQuery(id)
        {
            PaginationParameters = paginationParameters
        };
        
        var result = await Sender.Send(query, cancellationToken);
        
        Response.AddPaginationHeader(new PageDescriptor(result.Comments.PageIndex, result.Comments.PageSize, result.Comments.ItemTotal, result.Comments.TotalPages));
        
        return Ok(result);
    }
    
    [HttpGet("like/{id}")]
    public async Task<IActionResult> GetCommentLikes(int id, CancellationToken cancellationToken)
    {
        var query = new GetCommentLikesQuery(id);
        
        var result = await Sender.Send(query, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadComment(UploadCommentCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("like/{id}")]
    public async Task<IActionResult> LikeComment(int id, CancellationToken cancellationToken)
    {
        var command = new LikeCommentCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCommentCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpDelete("unlike/{id}")]
    public async Task<IActionResult> UnlikeComment(int id, CancellationToken cancellationToken)
    {
        var command = new UnlikeCommentCommand(id);
        
        var result = await Sender.Send(command, cancellationToken);
        
        return Ok(result);
    }
}