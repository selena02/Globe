using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Likes.Queries.GetPostLikes;

public record GetPostLikesQuery(int PostId) : IRequest<GetPostLikesResponse>;

public record GetPostLikesResponse(List<LikedUserDto> LikedUsers);

public class GetPostLikesQueryHandler : IRequestHandler<GetPostLikesQuery, GetPostLikesResponse>
{
    private readonly IApplicationDbContext _context;

    public GetPostLikesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetPostLikesResponse> Handle(GetPostLikesQuery request, CancellationToken cancellationToken)
    {
        var likedUsers = await _context.Likes
            .Where(l => l.PostId == request.PostId)
            .Select(l => new LikedUserDto(
                l.UserId,
                l.User.UserName,
                l.User.PicturePublicId))
            .ToListAsync(cancellationToken);

        return new GetPostLikesResponse(likedUsers);
    }
}
