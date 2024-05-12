using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Comments.Commands;

public record UploadCommentCommand : ICommand<UploadCommentResponse>
{
    public string? Content { get; set; }
    public int PostId { get; set; }
}

public record UploadCommentResponse(CommentDto Comment);

public class UploadCommentCommandHandler : ICommandHandler<UploadCommentCommand, UploadCommentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    
    public UploadCommentCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<UploadCommentResponse> Handle(UploadCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        
        if (currentUser is null)
        {
            throw new UnauthorizedException("User not authenticated");
        }
        
        var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken);
        
        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }
        
        var comment = new Comment
        {
            Text = request.Content,
            PostId = request.PostId,
            UserId = currentUser.Id
        };
        
        await _context.Comments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        var newComment = new CommentDto(
            comment.CommentId,
            comment.UserId,
            comment.User.UserName,
            comment.User.ProfilePictureUrl,
            comment.Text,
            comment.LikesCount,
            comment.CreatedAt,
            comment.Likes.Any(l => l.UserId == currentUser.Id),
            comment.UserId == currentUser.Id
        );
        
        return new UploadCommentResponse(newComment);
    }
}