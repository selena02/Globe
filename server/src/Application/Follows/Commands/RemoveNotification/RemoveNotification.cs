using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Follows.Commands.RemoveNotification;

public record RemoveNotificationCommand(int NotificationId) : ICommand<RemoveNotificationResponse>;

public record RemoveNotificationResponse(bool NotificationRemoved);

public class RemoveNotificationCommandHandler : ICommandHandler<RemoveNotificationCommand, RemoveNotificationResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public RemoveNotificationCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<RemoveNotificationResponse> Handle(RemoveNotificationCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        var notification = await _context.Notifications
            .SingleOrDefaultAsync(n => n.NotificationId == request.NotificationId && n.RecieverId == currentUserId, cancellationToken);

        if (notification is null)
        {
            return new RemoveNotificationResponse(false);
        }

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return new RemoveNotificationResponse(true);
    }
}