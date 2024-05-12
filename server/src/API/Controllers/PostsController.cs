using API.Helpers;
using Application.Common.Models;
using Application.Common.Utils;
using Application.Likes.Commands.LikePost;
using Application.Likes.Commands.UnlikePost;
using Application.Posts.Commands.DeletePost;
using Application.Posts.Commands.EditPost;
using Application.Posts.Commands.UploadPost;
using Application.Posts.Queries.GetPostById;
using Application.Posts.Queries.GetPosts;
using Application.Posts.Queries.GetUserPosts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PostsController(ISender sender) : BaseController(sender)
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery]PaginationParameters paginationParameters ,CancellationToken cancellationToken)
    {
        var query = new GetPostsQuery
        {
            PaginationParameters = paginationParameters
        };

        var result = await Sender.Send(query, cancellationToken);
        
        Response.AddPaginationHeader(new PageDescriptor (result.Posts.PageIndex, result.Posts.PageSize, result.Posts.ItemTotal, result.Posts.TotalPages));
        
        return Ok(result);
    }
    
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserPosts(int id, [FromQuery]PaginationParameters paginationParameters, CancellationToken cancellationToken)
    {
        var query = new GetUserPostsQuery(id)
        {
            PaginationParameters = paginationParameters
        };

        var result = await Sender.Send(query, cancellationToken);
        
        Response.AddPaginationHeader(new PageDescriptor (result.Posts.PageIndex, result.Posts.PageSize, result.Posts.ItemTotal, result.Posts.TotalPages));
        
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(int id, CancellationToken cancellationToken)
    {
        var query = new GetPostByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("upload")]
    public async Task<IActionResult> UploadPost([FromForm]UploadPostCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("like/{id}")]
    public async Task<IActionResult> LikePost(int id, CancellationToken cancellationToken)
    {
        var command = new LikePostCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpPut("edit")]
    public async Task<IActionResult> EditPost([FromForm]EditPostCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePost(int id, CancellationToken cancellationToken)
    {
        var command = new DeletePostCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
    [HttpDelete("unlike/{id}")]
    public async Task<IActionResult> UnlikePost(int id, CancellationToken cancellationToken)
    {
        var command = new UnlikePostCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return Ok(result);
    }
    
}