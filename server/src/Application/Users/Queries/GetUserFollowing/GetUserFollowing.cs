using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Users.Queries.GetUserFollowing;

public record GetUserFollowingQuery(int UserId) : IQuery<GetUserFollowingResponse>;

public record GetUserFollowingResponse(List<FollowerDto> Following); 

public class GetUserFollowingQueryHandler : IQueryHandler<GetUserFollowingQuery, GetUserFollowingResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserFollowingQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserFollowingResponse> Handle(GetUserFollowingQuery request, CancellationToken cancellationToken)
    {
        var following = await _context.Follows
            .Where(f => f.FollowerId == request.UserId)
            .Select(f => new FollowerDto(f.FollowingId, f.Following.UserName, f.Following.PicturePublicId))
            .ToListAsync(cancellationToken);

        return new GetUserFollowingResponse(following);
    }
}