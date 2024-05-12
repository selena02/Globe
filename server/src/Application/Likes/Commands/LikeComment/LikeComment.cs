using System.Windows.Input;
using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Likes.Commands.LikeComment;

public record LikeCommentCommand(int CommentId) : ICommand<LikeCommentResponse>;

public record LikeCommentResponse(bool IsLiked);

public class LikePostCommandHandler : ICommandHandler<LikeCommentCommand, LikeCommentResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public LikePostCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<LikeCommentResponse> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        if (currentUserId is null)
        {
            throw new UnauthorizedException("User not authenticated");
        }

        var comment = await _context.Comments.FindAsync(new object[] { request.CommentId }, cancellationToken);

        if (comment is null)
        {
            throw new NotFoundException("Comment not found");
        }

        var like = await _context.Likes.FirstOrDefaultAsync(l => l.CommentId == request.CommentId && l.UserId == currentUserId, cancellationToken);

        if (like is not null)
        {
            return new LikeCommentResponse(true);
        }

        comment.LikesCount++;

        like = new Like
        {
            CommentId = request.CommentId,
            UserId = currentUserId
        };

        await _context.Likes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new LikeCommentResponse(true);
    }
}