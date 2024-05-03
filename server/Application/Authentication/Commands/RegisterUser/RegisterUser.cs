using Application.Authentication.DTOs;
using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Authentication.Commands.RegisterUser;

public record RegisterUserCommand(RegisterDto RegisterDto) : ICommand<AuthResponseDto>;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IJWTService _tokenService;

    public RegisterUserCommandHandler(IApplicationDbContext context,
        IIdentityService identityService, IJWTService tokenService)
    {
        _context = context;
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.BeginTransactionAsync(cancellationToken);

        try
        {
            var createdResult = await _identityService.CreateAsync(command.RegisterDto, command.RegisterDto.Password);
            
            if (!createdResult.Succeeded)
            {
                if (createdResult.Errors.Any(e => e.Code == "DuplicateUserName"))
                {
                    throw new ConflictException("Username is taken");
                }
                
                if (createdResult.Errors.Any(e => e.Code == "DuplicateEmail"))
                {
                    throw new ConflictException("Email is taken");
                }

                throw new ServerErrorException("Error registering user");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserName == command.RegisterDto.UserName, cancellationToken);

            if (user == null)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new NotFoundException("Error registering user");
            }

            var roleResult = await _identityService.AddToRoleAsync(user, Roles.Traveller.ToString());

        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new ServerErrorException("Error registering user");
        }
        
        return new AuthResponseDto(null, null, null);
    }
}