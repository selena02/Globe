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
        
        var notifications = await _context.Notifications
            .Where(n => n.RecieverId == currentUserId)
            .Select(n => new FollowNotificationDto(
                n.NotificationId,
                n.UserId,
                n.User.UserName,
                n.User.PicturePublicId,
                n.DateCreated))
            .ToListAsync(cancellationToken);

        return new GetUserFollowNotificationsResponse(notifications);
    }
}
