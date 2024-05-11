using Application.Authentication.DTOs;
using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Exceptions;

namespace Application.Authentication.Commands.LoginUser;

public record LoginUserCommand(LoginDto LoginDto) : ICommand<AuthResponseDto>;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthResponseDto>
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

    public async Task<AuthResponseDto> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == command.LoginDto.Email, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid credentials");
        }
        
        var isValidPassword = await _identityService.CheckPasswordAsync(user, command.LoginDto.Password);
        
        if (!isValidPassword)
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        var token = await _tokenService.GenerateTokenAsync(user);
        
        var roles = await _identityService.GetRolesAsync(user);

        return new AuthResponseDto(token, user.Id, roles);
    }
}    