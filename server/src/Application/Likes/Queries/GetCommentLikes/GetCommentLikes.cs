using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;

namespace Application.Comments.Queries.GetCommentLikes;

public record GetCommentLikesQuery(int CommentId) : IQuery<GetCommentLikesResponse>;

public record GetCommentLikesResponse(List<LikedUserDto> LikedUsers);

public class GetCommentLikesQueryHandler : IQueryHandler<GetCommentLikesQuery, GetCommentLikesResponse>
{
    private readonly IApplicationDbContext _context;

    public GetCommentLikesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetCommentLikesResponse> Handle(GetCommentLikesQuery request, CancellationToken cancellationToken)
    {
        var likedUsers = await _context.Likes
            .Where(l => l.CommentId == request.CommentId)
            .Select(l => new LikedUserDto(
                l.UserId,
                l.User.UserName,
               l.User.PicturePublicId))
            .ToListAsync(cancellationToken);

        return new GetCommentLikesResponse(likedUsers);
    }
}