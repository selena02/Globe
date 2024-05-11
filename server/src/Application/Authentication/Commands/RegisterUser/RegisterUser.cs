using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Authentication.Commands.RegisterUser;

public class RegisterUserCommand : ICommand<RegisterUserResponse>
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FullName { get; set; }
} 

public record RegisterUserResponse(
    string Token,
    int? Id,
    IList<string> Roles
);

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
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

    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.BeginTransactionAsync(cancellationToken);

        try
        {
            var createdResult = await _identityService.CreateAsync(request, request.Password);

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

                throw new ServerErrorException("Error creating user");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserName == request.UserName, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("Error registering user");
            }

            var roleResult = await _identityService.AddToRoleAsync(user, Roles.Traveller.ToString());

            if (!roleResult.Succeeded)
            {
                throw new ServerErrorException("Error adding roles to user");
            }

            var token = await _tokenService.GenerateTokenAsync(user);

            var roles = await _identityService.GetRolesAsync(user);
            
            await transaction.CommitAsync(cancellationToken);
            
            return new RegisterUserResponse(token, user.Id, roles);

        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}