using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(int Id) : ICommand<DeleteCommentResponse>;

public record DeleteCommentResponse(bool IsDeleted);

public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand, DeleteCommentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public DeleteCommentCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<DeleteCommentResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        
        var comment = await _context.Comments.FindAsync(new object[] { request.Id }, cancellationToken);

        if (comment is null)
        {
            throw new NotFoundException("Comment not found");
        }
        
        var post = await _context.Posts.FindAsync(new object[] { comment.PostId }, cancellationToken);
        
        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }
        
        if (comment.UserId != currentUserId || !_authService.GetUserRoles().Contains(Roles.Guide.ToString()))
        {
            throw new ForbiddenAccessException("User is not authorized to delete this comment");
        }

        _context.Comments.Remove(comment);
        
        post.CommentsCount--;

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteCommentResponse(true);
    }
}
