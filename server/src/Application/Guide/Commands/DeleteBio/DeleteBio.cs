using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Guide.Commands.DeleteBio;

public record DeleteBioCommand(int UserId) : ICommand<DeleteBioRespose>;

public record DeleteBioRespose(bool IsDeleted);

public class DeleteBioCommandHandler : IRequestHandler<DeleteBioCommand, DeleteBioRespose>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly IIdentityService _identityService;

    public DeleteBioCommandHandler(IApplicationDbContext context, IAuthService authService, IIdentityService identityService)
    {
        _context = context;
        _authService = authService;
        _identityService = identityService;
    }

    public async Task<DeleteBioRespose> Handle(DeleteBioCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();

        if (currentUser is null)
        {
            throw new NotFoundException("Current user not found");
        }

        var isGuide = await _identityService.IsInRoleAsync(currentUser, Roles.Guide.ToString());

        if (!isGuide)
        {
            throw new ForbiddenAccessException("You are not authorized to delete a bio");
        }

        var user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User not found");
        }

        user.Bio = null;

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteBioRespose(true);
    }
} 
