using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Pilot.Commands.DeleteUser;

public record DeleteUserCommand(int UserId) : ICommand<DeleteUserResponse>;

public record DeleteUserResponse(bool UserDeleted);

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserResponse>
{
    private readonly IIdentityService _identityService;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService, IAuthService authService, IIdentityService identityService)
    {
        _userService = userService;
        _authService = authService;
        _identityService = identityService;
    }

    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();
        
        var isPilot = await _identityService.IsInRoleAsync(currentUser, Roles.Pilot.ToString());
        
        if (!isPilot)
        {
            throw new ForbiddenAccessException("Only pilots can delete other users");
        }
        
        _authService.EnsureRoleAccess(Roles.Pilot.ToString());
        
        await _userService.DeleteUserAsync(request.UserId, cancellationToken);

        return new DeleteUserResponse(true);
    }
}
