using Application.Common.Abstractions;
using Application.Common.Interfaces;

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

        _context.Follows.Remove(follow);

        await _context.SaveChangesAsync(cancellationToken);

        return new UnfollowUserResponse(true);
    }
}
