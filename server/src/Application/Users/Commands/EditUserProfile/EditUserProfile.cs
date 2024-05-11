﻿using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.EditUserProfile;
public class EditUserProfileCommand : ICommand<EditProfileResponse>
{
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}

public record class EditProfileResponse
(
    string? Location,
    string? Bio,
    string? ProfilePictureUrl
);

public class EditUserProfileCommandHandler : ICommandHandler<EditUserProfileCommand ,EditProfileResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IAuthService _authService;
    
    public EditUserProfileCommandHandler(IApplicationDbContext context, ICloudinaryService cloudinaryService, IAuthService authService)
    {
        _context = context;
        _cloudinaryService = cloudinaryService;
        _authService = authService;
    }
    
    public async Task<EditProfileResponse> Handle(EditUserProfileCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        
        if (currentUserId is null)
        {
            throw new UnauthorizedException("User not authenticated");
        }
        
        var user = await _context.Users.FindAsync( new object[] { currentUserId }, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        if (request.ProfilePicture is not null && request.ProfilePicture.Length > 0)
        {
            if(user.PicturePublicId is not null)
            {
                var deleteImageResult = await _cloudinaryService.DeleteImageAsync(user.PicturePublicId);
                
                if (deleteImageResult.Errors is not null)
                {
                    throw new ServerErrorException("Error deleting profile picture");
                }
            }
            
            var uploadImageResult = await _cloudinaryService.UploadProfileImage(request.ProfilePicture);
            
            if (uploadImageResult?.Errors is not null)
            {
                throw new ServerErrorException("Error uploading profile picture");
            }
            
            user.ProfilePictureUrl = uploadImageResult?.Url;
            user.PicturePublicId = uploadImageResult?.PublicId;
        }
        
        if (request.Location is not null)
        {
            user.Location = request.Location;
        }
        
        if (request.Bio is not null)
        {
            user.Bio = request.Bio;
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return new EditProfileResponse(user.Location, user.Bio, user.ProfilePictureUrl);
    }
}