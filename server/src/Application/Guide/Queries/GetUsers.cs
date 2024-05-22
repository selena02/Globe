using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Enums;

namespace Application.Guide.Queries;

public record GetUsersQuery() : IRequest<GetUsersResponse>;

public record GetUsersResponse(List<UserDto> Users, bool IsPilot);

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, GetUsersResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly IIdentityService _identityService;

    public GetUsersQueryHandler(IApplicationDbContext context, IAuthService authService, IIdentityService identityService)
    {
        _context = context;
        _authService = authService;
        _identityService = identityService;
    }

    public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync();

        var users = _context.Users
            .Where(u => u.Id != currentUser.Id)
            .Include((u => u.UserRoles))
            .ThenInclude(ur => ur.Role)
            .Select(u => new UserDto(
                u.Id,
                u.UserName,
                u.PicturePublicId,
                u.CreatedTime,
                u.UserRoles.Any(ur => ur.Role.Name == Roles.Guide.ToString())
            )).ToList();
        
        var isPilot = await _identityService.IsInRoleAsync(currentUser, Roles.Pilot.ToString());
        
        return new GetUsersResponse(users, isPilot);
    }
}