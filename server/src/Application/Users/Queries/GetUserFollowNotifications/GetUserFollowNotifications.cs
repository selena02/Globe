using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Users.Queries.GetUserFollowNotifications;

public record GetUserFollowNotificationsQuery() : IQuery<GetUserFollowNotificationsResponse>;
    
public record GetUserFollowNotificationsResponse(List<FollowNotificationDto> FollowNotifications);

public class GetUserFollowNotificationsQueryHandler : IQueryHandler<GetUserFollowNotificationsQuery, GetUserFollowNotificationsResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public GetUserFollowNotificationsQueryHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<GetUserFollowNotificationsResponse> Handle(GetUserFollowNotificationsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        
        var followNotifications = await _context.Follows
            .Where(f => f.FollowingId == currentUserId)
            .Select(f => new FollowNotificationDto(
                f.FollowerId, 
                f.Follower.UserName,
                f.Follower.PicturePublicId,
                f.FollowedDate))
            .ToListAsync(cancellationToken);
        
        _context.Notifications.RemoveRange(_context.Notifications.Where(n => n.UserId == currentUserId));
        await _context.SaveChangesAsync(cancellationToken);

        return new GetUserFollowNotificationsResponse(followNotifications);
    }
}
