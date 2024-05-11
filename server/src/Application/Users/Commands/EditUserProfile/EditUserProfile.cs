using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Domain.Exceptions;

namespace Application.Users.Commands.EditUserProfile;
public record EditUserProfileCommand(EditProfileDto EditProfileDto) : ICommand<EditProfileResponseDto>;

public class EditUserProfileCommandHandler : ICommandHandler<EditUserProfileCommand ,EditProfileResponseDto>
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
    
    public async Task<EditProfileResponseDto> Handle(EditUserProfileCommand command, CancellationToken cancellationToken)
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
        
        if (command.EditProfileDto.ProfilePicture is not null && command.EditProfileDto.ProfilePicture.Length > 0)
        {
            if(user.PicturePublicId is not null)
            {
                var deleteImageResult = await _cloudinaryService.DeleteImageAsync(user.PicturePublicId);
                
                if (deleteImageResult.Errors is not null)
                {
                    throw new ServerErrorException("Error deleting profile picture");
                }
            }
            
            var uploadImageResult = await _cloudinaryService.UploadProfileImage(command.EditProfileDto.ProfilePicture);
            
            if (uploadImageResult?.Errors is not null)
            {
                throw new ServerErrorException("Error uploading profile picture");
            }
            
            user.ProfilePictureUrl = uploadImageResult?.Url;
            user.PicturePublicId = uploadImageResult?.PublicId;
        }
        
        if (command.EditProfileDto.Location is not null)
        {
            user.Location = command.EditProfileDto.Location;
        }
        
        if (command.EditProfileDto.Bio is not null)
        {
            user.Bio = command.EditProfileDto.Bio;
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return new EditProfileResponseDto(user.Location, user.Bio, user.ProfilePictureUrl);
    }
}