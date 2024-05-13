using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Exceptions;

namespace Application.Likes.Commands.UnlikePost;

public record UnlikePostCommand(int PostId) : ICommand<UnlikePostResponse>;

public record UnlikePostResponse(bool IsUnliked);

public class UnlikePostCommandHandler : ICommandHandler<UnlikePostCommand, UnlikePostResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public UnlikePostCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<UnlikePostResponse> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        if (currentUserId is null)
        {
            throw new UnauthorizedException("User not authenticated");
        }
        
        var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken);

        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }
        
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == request.PostId && l.UserId == currentUserId, cancellationToken);

        if (like is null)
        {
            return new UnlikePostResponse(true);
        }
        
        _context.Likes.Remove(like);
        
        post.LikesCount--;

        await _context.SaveChangesAsync(cancellationToken);

        return new UnlikePostResponse(true);
    }
}