using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Posts.Commands.DeletePost;

public record DeletePostCommand(int Id) : ICommand<DeletePostResponse>;

public record DeletePostResponse(bool IsDeleted);

public class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, DeletePostResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public DeletePostCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        var post = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }

        if (post.UserId != currentUserId || _authService.GetUserRoles().Contains(Roles.Guide.ToString()))
        {
            throw new ForbiddenAccessException("User is not authorized to delete this post");
        }

        _context.Posts.Remove(post);

        await _context.SaveChangesAsync(cancellationToken);

        return new DeletePostResponse(true);
    }
}