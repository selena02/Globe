using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Users.Commands.DeleteAccount;

public record DeleteAccountCommand : ICommand<DeleteAccountResponse>;

public record DeleteAccountResponse(bool AccountDeleted);

public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand, DeleteAccountResponse>
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public DeleteAccountCommandHandler(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    public async Task<DeleteAccountResponse> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        
        await _userService.DeleteUserAsync(currentUserId, cancellationToken);

        return new DeleteAccountResponse(true);
    }
}