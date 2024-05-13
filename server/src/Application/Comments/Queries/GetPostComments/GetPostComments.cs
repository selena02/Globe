using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Utils;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Comments.Queries.GetPostComments;

public record GetPostCommentsQuery(int Id) : IQuery<GetPostCommentsResponse>
{
    public PaginationParameters PaginationParameters { get; set; } = new PaginationParameters();
}

public record GetPostCommentsResponse(PaginatedList<CommentDto> Comments);

public class GetPostCommentsQueryHandler : IQueryHandler<GetPostCommentsQuery, GetPostCommentsResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public GetPostCommentsQueryHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<GetPostCommentsResponse> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
    {
        
        var currentUserId = _authService.GetCurrentUserId();

        var post = await _context.Posts
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.PostId == request.Id, cancellationToken: cancellationToken);
            

        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }
        
        var comments = _context.Comments
            .AsQueryable()
            .Include(c => c.User)
            .Where(c => c.PostId == request.Id)
            .OrderByDescending<Comment, DateTime>(c => c.CreatedAt)
            .Select(c => new CommentDto(
                c.CommentId,
                c.UserId,
                c.User.UserName,
                c.User.PicturePublicId,
                c.Text,
                c.LikesCount,
                c.CreatedAt,
                c.Likes.Any(l => l.UserId == currentUserId),
                c.UserId == currentUserId || _authService.GetUserRoles().Contains(Roles.Guide.ToString())
            ));

        var pagedComments = await PaginatedList<CommentDto>
            .CreateAsync(comments, request.PaginationParameters.PageNumber, request.PaginationParameters.PageSize);

        return new GetPostCommentsResponse(pagedComments);
    }
}