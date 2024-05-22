using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Landmarks.Queries.GetUserLandmarks;

public record GetUserLandmarksQuery(int UserId) : IQuery<GetUserLandmarksResponse>;

public record GetUserLandmarksResponse(List<LandmarkDto> Landmarks);

public record LandmarkDto(
    int LandmarkId,
    string LocationName, 
    string LandmarkPictureId,
    int Rating,
    DateTime VisitedOn);

public class GetUserLandmarksQueryHandler : IQueryHandler<GetUserLandmarksQuery, GetUserLandmarksResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserLandmarksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserLandmarksResponse> Handle(GetUserLandmarksQuery request, CancellationToken cancellationToken)
    {
        var landmarks = await _context.Landmarks
            .Where(l => l.UserId == request.UserId)
            .Select(l => new LandmarkDto(
                l.LandmarkId,
                l.LocationName,
                l.PublicId,
                l.Rating,
                l.VisitedOn))
            .ToListAsync(cancellationToken);

        return new GetUserLandmarksResponse(landmarks);
    }
}