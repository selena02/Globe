using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utils;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Posts.Queries.GetUserPosts;

public record GetUserPostsQuery(int Id) : IQuery<GetUserPostsResponse>
{
    public PaginationParameters PaginationParameters { get; set; } = new PaginationParameters();
}

public record GetUserPostsResponse(PaginatedList<PostDto> Posts);

public class GetUserPostsQueryHandler : IQueryHandler<GetUserPostsQuery, GetUserPostsResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    
    public GetUserPostsQueryHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<GetUserPostsResponse> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync( new object[] { request.Id }, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        var posts = _context.Posts
            .AsQueryable()
            .Include(p => p.User)
            .Where(p => p.UserId == request.Id)
            .OrderByDescending<Post, DateTime>(p => p.CreatedAt)
            .Select(p => new PostDto(
                p.UserId,
                p.User.UserName,
                p.User.ProfilePictureUrl,
                p.LikesCount,
                p.CommentsCount,
                p.PostId,
                p.PhotoUrl
            ));
        
        var pagedPosts = await PaginatedList<PostDto>
            .CreateAsync(posts, request.PaginationParameters.PageNumber, request.PaginationParameters.PageSize);
        
        return new GetUserPostsResponse(pagedPosts);
    }
}