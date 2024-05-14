using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Guide.Commands;

public record DeletePictureCommand(int UserId) : ICommand<DeletePictureResponse>;

public record DeletePictureResponse(bool IsDeleted);

public class DeletePictureCommandHandler : IRequestHandler<DeletePictureCommand, DeletePictureResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly IIdentityService _identityService;
    private readonly ICloudinaryService _cloudinaryService;
    
    public DeletePictureCommandHandler(IApplicationDbContext context, 
        IAuthService authService, IIdentityService identityService, ICloudinaryService cloudinaryService)
    {
        _context = context;
        _authService = authService;
        _identityService = identityService;
        _cloudinaryService = cloudinaryService;
    }
    
    public async Task<DeletePictureResponse> Handle(DeletePictureCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        
        if (currentUser is null)
        {
            throw new NotFoundException("Current user not found");
        }

        var isGuide = await _identityService.IsInRoleAsync(currentUser, Roles.Guide.ToString());

        if (!isGuide)
        {
            throw new ForbiddenAccessException("You are not authorized to delete a picture");
        }
        
        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        if (!string.IsNullOrEmpty(user.PicturePublicId))
        {
            var deletionResult = await _cloudinaryService.DeleteImageAsync(user.PicturePublicId);

            if (deletionResult.Errors is not null)
            {
                throw new ServerErrorException(deletionResult.Errors);
            }
        }
        else
        {
            return new DeletePictureResponse(false);
        }
        
        user.PicturePublicId = null;
        user.ProfilePictureUrl = null;
        
        await _context.SaveChangesAsync(cancellationToken);

        return new DeletePictureResponse(true);
    }
}
