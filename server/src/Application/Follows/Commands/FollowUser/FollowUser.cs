using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Follows.Commands.FollowUser;

public record FollowUserCommand(int UserId) : ICommand<FollowUserResponse>;

public record FollowUserResponse(bool IsFollowed);

public class FollowUserCommandHandler : ICommandHandler<FollowUserCommand, FollowUserResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public FollowUserCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<FollowUserResponse> Handle(FollowUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        
        if (currentUserId == request.UserId)
        {
            throw new BadRequestException("You cannot follow yourself");
        }
        
        var follow = await _context.Follows
            .FirstOrDefaultAsync(f => f.FollowerId == currentUserId && f.FollowingId == request.UserId, cancellationToken);

        if (follow is not null)
        {
            return new FollowUserResponse(true);
        }

        var newFollow = new Follow
        {
            FollowerId = currentUserId,
            FollowingId = request.UserId
        };
        
        var currentUser = await _context.Users.FindAsync(new object[] { currentUserId }, cancellationToken);
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        
        if (currentUser is null || user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        currentUser.FollowingCount++;
        user.FollowersCount++;
        _context.Follows.Add(newFollow);
        
        var notification = new Notification
        {
            UserId = currentUserId, 
            RecieverId = request.UserId,
        };
        
        _context.Notifications.Add(notification);

        await _context.SaveChangesAsync(cancellationToken);

        return new FollowUserResponse(true);
    }
}