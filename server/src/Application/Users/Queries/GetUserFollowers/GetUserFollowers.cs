using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Users.Queries.GetUserFollowers;

public record GetUserFollowersQuery(int UserId) : IRequest<GetUserFollowersResponse>;

public record GetUserFollowersResponse(List<FollowerDto> Followers);

public class GetUserFollowersQueryHandler : IRequestHandler<GetUserFollowersQuery, GetUserFollowersResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserFollowersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserFollowersResponse> Handle(GetUserFollowersQuery request, CancellationToken cancellationToken)
    {
        var followers = await _context.Follows
            .Where(f => f.FollowerId == request.UserId)
            .Select(f => new FollowerDto(f.FollowerId, f.Follower.UserName, f.Follower.ProfilePictureUrl))
            .ToListAsync(cancellationToken);

        return new GetUserFollowersResponse(followers);
    }
}
