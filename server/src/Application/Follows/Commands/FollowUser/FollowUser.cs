using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Entities;

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
        
        await _context.Follows.AddAsync(newFollow, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new FollowUserResponse(true);
    }
}