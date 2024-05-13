using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Exceptions;

namespace Application.Likes.Commands.UnlikeComment;

public record UnlikeCommentCommand(int CommentId) : ICommand<UnlikeCommentResponse>;

public record UnlikeCommentResponse(bool IsUnliked);

public class UnlikeCommentCommandHandler : ICommandHandler<UnlikeCommentCommand, UnlikeCommentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public UnlikeCommentCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<UnlikeCommentResponse> Handle(UnlikeCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        
        var comment = await _context.Comments.FindAsync(new object[] { request.CommentId }, cancellationToken);

        if (comment is null)
        {
            throw new NotFoundException("Comment not found");
        }
        
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.CommentId == request.CommentId && l.UserId == currentUserId, cancellationToken);

        if (like is null)
        {
            return new UnlikeCommentResponse(true);
        }
        
        _context.Likes.Remove(like);
        
        comment.LikesCount--;

        await _context.SaveChangesAsync(cancellationToken);

        return new UnlikeCommentResponse(true);
    }
}
