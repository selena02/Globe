using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Exceptions;

namespace Application.Follows.Commands.UnfollowUser;

public record UnfollowUserCommand(int UserId) : ICommand<UnfollowUserResponse>;

public record UnfollowUserResponse(bool IsUnfollowed);

public class UnfollowUserCommandHandler : ICommandHandler<UnfollowUserCommand, UnfollowUserResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public UnfollowUserCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<UnfollowUserResponse> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        var follow = await _context.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == currentUserId && f.FollowingId == request.UserId, cancellationToken);

        if (follow is null)
        {
            return new UnfollowUserResponse(false);
        }
        
        var currentUser = await _context.Users.FindAsync(new object[] { currentUserId }, cancellationToken);
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        
        if (currentUser is null || user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        currentUser.FollowingCount--;
        user.FollowersCount--;
        _context.Follows.Remove(follow);
        
        var existingNotification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.UserId == currentUserId && n.RecieverId == request.UserId, cancellationToken);
        
        if (existingNotification is not null)
        {
            _context.Notifications.Remove(existingNotification);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UnfollowUserResponse(true);
    }
}
