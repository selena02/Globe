using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Domain.Enums;
using Domain.Exceptions;

namespace Application.Landmarks.Queries.GetLandmarkById;

public record GetLandmarkByIdQuery (int LandmarkId) : IQuery<LandmarkByIdResponse>;

public record LandmarkByIdResponse(
    int LandmarkId,
    string LocationName,
    DateTime VisitedOn,
    string Review,
    string PublicId,
    int Rating,
    string Longitude,
    string Latitude,
    string Country,
    string City,
    bool CanDelete
);

public class GetLandmarkByIdCommandHandler : IQueryHandler<GetLandmarkByIdQuery, LandmarkByIdResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;

    public GetLandmarkByIdCommandHandler(IApplicationDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<LandmarkByIdResponse> Handle(GetLandmarkByIdQuery request, CancellationToken cancellationToken)
    {
        var landmark = await _context.Landmarks
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.LandmarkId == request.LandmarkId, cancellationToken);

        if (landmark is null)
        {
            throw new NotFoundException("Landmark not found");
        }

        var currentUserId = _authService.GetCurrentUserIdOrNull();
        var currentUserRoles = _authService.GetUserRoles();

        var isOwner = landmark.UserId == currentUserId;
        var canDelete = isOwner || currentUserRoles.Contains(Roles.Guide.ToString());

        return new LandmarkByIdResponse(
            landmark.LandmarkId,
            landmark.LocationName,
            landmark.VisitedOn,
            landmark.Review,
            landmark.PublicId,
            landmark.Rating,
            landmark.Longitude,
            landmark.Latitude,
            landmark.Country,
            landmark.City,
            canDelete
        );
    }
}
    