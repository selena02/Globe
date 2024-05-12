using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Posts.Queries.GetPostById;

public record GetPostByIdQuery(int Id) : IQuery<PostByIdResponse>;

public record PostByIdResponse(
    int PostId,
    string Content,
    string PostPictureUrl,
    DateTime CreatedAt,
    int LikesCount,
    int CommentsCount,
    int UserId,
    string UserName,
    string? ProfilePictureUrl,
    bool? IsLiked,
    bool IsOwner,
    bool CanDelete
);

public class GetPostByIdCommandHandler : IQueryHandler<GetPostByIdQuery, PostByIdResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    
    public GetPostByIdCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<PostByIdResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.PostId == request.Id, cancellationToken);
        
        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }
        
        var currentUserId = _authService.GetCurrentUserId();
        var currentUserRoles = _authService.GetUserRoles();
        
        var isLiked = post.Likes is not null && post.Likes.Any(l => l.UserId == currentUserId);
        var isOwner = post.UserId == currentUserId;
        var canDelete = isOwner || currentUserRoles.Contains(Roles.Guide.ToString());
        
        return new PostByIdResponse(
            post.PostId,
            post.Content,
            post.PhotoUrl,
            post.CreatedAt,
            post.LikesCount,
            post.CommentsCount,
            post.UserId,
            post.User.UserName,
            post.User.ProfilePictureUrl,
            isLiked,
            isOwner,
            canDelete
        );
    }
}
