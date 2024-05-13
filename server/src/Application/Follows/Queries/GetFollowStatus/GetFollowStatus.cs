using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Follows.Queries.GetFollowStatus;

public record GetFollowStatusQuery(int UserId) : IQuery<GetFollowStatusResponse>;

public record GetFollowStatusResponse(bool IsFollowing);

public class GetFollowStatusQueryHandler : IQueryHandler<GetFollowStatusQuery, GetFollowStatusResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    
    public GetFollowStatusQueryHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public Task<GetFollowStatusResponse> Handle(GetFollowStatusQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        
        var isFollowing = _context.Follows
            .Any(f => f.FollowerId == currentUserId && f.FollowingId == request.UserId);
        
        return Task.FromResult(new GetFollowStatusResponse(isFollowing));
    }
}
