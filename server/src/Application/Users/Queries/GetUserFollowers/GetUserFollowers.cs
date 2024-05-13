using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Users.Queries.GetUserFollowers;

public record GetUserFollowersQuery(int UserId) : IQuery<GetUserFollowersResponse>;

public record GetUserFollowersResponse(List<FollowerDto> Followers);

public class GetUserFollowersQueryHandler : IQueryHandler<GetUserFollowersQuery, GetUserFollowersResponse>
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
