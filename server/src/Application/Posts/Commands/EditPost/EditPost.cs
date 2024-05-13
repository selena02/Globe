using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands.EditPost;

public class EditPostCommand : ICommand<EditPostResponse>
{
    public int PostId { get; set; }
    public string? Content { get; set; }
    public IFormFile? PostPicture { get; set; }
}

public record EditPostResponse
(
    string? Content,
    string? PostPictureUrl
);

public class EditPostCommandHandler : ICommandHandler<EditPostCommand, EditPostResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IAuthService _authService;

    public EditPostCommandHandler(IApplicationDbContext context, ICloudinaryService cloudinaryService, IAuthService authService)
    {
        _context = context;
        _cloudinaryService = cloudinaryService;
        _authService = authService;
    }

    public async Task<EditPostResponse> Handle(EditPostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();

        var post = await _context.Posts.FindAsync(new object[] { request.PostId }, cancellationToken);

        if (post is null)
        {
            throw new NotFoundException("Post not found");
        }

        if (post.UserId != currentUserId)
        {
            throw new ForbiddenAccessException("User is not the owner of the post");
        }

        if (request.PostPicture is not null && request.PostPicture.Length > 0)
        {
            if (post.PublicId is not null)
            {
                var deleteImageResult = await _cloudinaryService.DeleteImageAsync(post.PublicId);

                if (deleteImageResult.Errors is not null)
                {
                    throw new ServerErrorException("Error deleting post photo");
                }
            }

            var uploadResult = await _cloudinaryService.UploadLandmarkImage(request.PostPicture);

            if (uploadResult?.Errors is not null)
            {
                throw new ServerErrorException("Error uploading post photo");
            }

            post.PhotoUrl = uploadResult?.Url;
            post.PublicId = uploadResult?.PublicId;
        }

        if (request.Content is not null)
        {
            post.Content = request.Content;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new EditPostResponse(post.Content, post.PhotoUrl);
    }
}