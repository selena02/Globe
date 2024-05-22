using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Pilot.Commands.UpdateRole;

public record UpdateRoleCommand(int UserId) : IQuery<UpdateRoleResponse>;

public record UpdateRoleResponse(bool IsGuide);

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, UpdateRoleResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly IIdentityService _identityService;

    public UpdateRoleCommandHandler(IApplicationDbContext context, IAuthService authService, IIdentityService identityService)
    {
        _context = context;
        _authService = authService;
        _identityService = identityService;
    }

    public async Task<UpdateRoleResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();

        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        var isGuide = user.UserRoles.Any(ur => ur.Role.Name == Roles.Guide.ToString());

        if (isGuide)
        {
            await _identityService.RemoveFromRoleAsync(user, Roles.Guide.ToString());
        }
        else
        {
            await _identityService.AddToRoleAsync(user, Roles.Guide.ToString());
        }

        return new UpdateRoleResponse(!isGuide);
    }
}