using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands.UploadPost;

public record UploadPostCommand : ICommand<UploadPostResponse>
{ 
   public string? Content { get; set; }
   public IFormFile? PostImage { get; set; }
}

public record UploadPostResponse(PostDto Post);

public class UploadPostCommandHandler : ICommandHandler<UploadPostCommand, UploadPostResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IAuthService _authService;
    
    public UploadPostCommandHandler(IApplicationDbContext context, ICloudinaryService cloudinaryService, IAuthService authService)
    {
        _context = context;
        _cloudinaryService = cloudinaryService;
        _authService = authService;
    }

    public async Task<UploadPostResponse> Handle(UploadPostCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        
        if (currentUser is null)
        {
            throw new UnauthorizedException("User not authenticated");
        }
        
        var uploadImageResult = await _cloudinaryService.UploadLandmarkImage(request.PostImage);
        
        if (uploadImageResult?.Errors is not null)
        {
            throw new ServerErrorException("Error uploading post picture");
        }
        
        var post = new Post
        {
            Content = request.Content,
            PhotoUrl = uploadImageResult?.Url,
            PublicId = uploadImageResult?.PublicId,
            UserId = currentUser.Id
        };
         
        await _context.Posts.AddAsync(post, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var newPost =  new PostDto(
            post.UserId,
            post.User.UserName,
            post.User.PicturePublicId,
            post.LikesCount,
            post.CommentsCount,
            post.PostId,
            post.PublicId,
            false
        );
        
        return new UploadPostResponse(newPost);
    }
}