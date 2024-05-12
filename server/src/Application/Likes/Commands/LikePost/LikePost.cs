using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Likes.Commands.LikePost;

public record LikePostCommand(int PostId) : ICommand<LikePostResponse>;

public record LikePostResponse(bool IsLiked);

public class LikePostCommandHandler : ICommandHandler<LikePostCommand, LikePostResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public LikePostCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<LikePostResponse> Handle(LikePostCommand request, CancellationToken cancellationToken)
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

        if (like is not null)
        {
            return new LikePostResponse(true);
        }
        
        post.LikesCount++;
        
        like = new Like
        {
            PostId = request.PostId,
            UserId = currentUserId
        };
        
        await _context.Likes.AddAsync(like, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new LikePostResponse(true);
    }
}