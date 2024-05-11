using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Exceptions;

namespace Application.Authentication.Commands.LoginUser;

public record LoginUserCommand(string? Email, string? Password) : ICommand<LoginUserResponse>;

public record LoginUserResponse(
    string Token,
    int? Id,
    IList<string> Roles,
    string? ProfilePictureUrl
);

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IJWTService _tokenService;

    public LoginUserCommandHandler(IApplicationDbContext context,
        IIdentityService identityService, IJWTService tokenService)
    {
        _context = context;
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<LoginUserResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == request.Email, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid credentials");
        }
        
        var isValidPassword = await _identityService.CheckPasswordAsync(user, request.Password);
        
        if (!isValidPassword)
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        var token = await _tokenService.GenerateTokenAsync(user);
        
        var roles = await _identityService.GetRolesAsync(user);

        return new LoginUserResponse(token, user.Id, roles, user.ProfilePictureUrl);
    }
}    