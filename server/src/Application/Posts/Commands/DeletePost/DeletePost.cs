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
    private readonly ICloudinaryService _cloudinaryService;

    public DeletePostCommandHandler(IApplicationDbContext context, IAuthService authService, ICloudinaryService cloudinaryService)
    {
        _context = context;
        _authService = authService;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        var post = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }
        
        var isGuide = _authService.GetUserRoles().Contains(Roles.Guide.ToString());

        if (post.UserId != currentUserId || isGuide)
        {
            throw new ForbiddenAccessException("User is not authorized to delete this post");
        }
        
        if (!string.IsNullOrEmpty(post.PublicId))
        {
            var deletionResult = await _cloudinaryService.DeleteImageAsync(post.PublicId);

            if (deletionResult.Errors is not null)
            {
                throw new ServerErrorException(deletionResult.Errors);
            }
        }
        
        _context.Posts.Remove(post);

        await _context.SaveChangesAsync(cancellationToken);

        return new DeletePostResponse(true);
    }
}