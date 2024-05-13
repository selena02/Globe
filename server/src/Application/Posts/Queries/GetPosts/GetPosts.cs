using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utils;

namespace Application.Posts.Queries.GetPosts;

public class GetPostsQuery : IQuery<GetPostsResponse>
{
    public PaginationParameters PaginationParameters { get; set; } = new PaginationParameters();
}

public record GetPostsResponse(PaginatedList<PostDto> Posts);

public class GetPostsQueryHandler : IQueryHandler<GetPostsQuery, GetPostsResponse>
{
    private readonly IApplicationDbContext _context;
    
    public GetPostsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetPostsResponse> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = _context.Posts
            .AsQueryable()
            .Include(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PostDto(
                p.UserId,
                p.User.UserName,
                p.User.PicturePublicId,
                p.LikesCount,
                p.CommentsCount,
                p.PostId,
                p.PublicId
            ));
        
        var pagedPosts = await PaginatedList<PostDto>
            .CreateAsync(posts, request.PaginationParameters.PageNumber, request.PaginationParameters.PageSize);
        
        return new GetPostsResponse(pagedPosts);
    }
}