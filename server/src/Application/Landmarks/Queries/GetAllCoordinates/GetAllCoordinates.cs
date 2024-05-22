using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Landmarks.Queries.GetAllCoordinates;

public record GetAllCoordinates() : IQuery<GetAllCoordinatesResponse>;

public record GetAllCoordinatesResponse(List<CoordinateDto> Coordinates);

public record CoordinateDto(string Latitude, string Longitude); 

public class GetAllCoordinatesHandler : IQueryHandler<GetAllCoordinates, GetAllCoordinatesResponse>
{
    private readonly IApplicationDbContext _context;
    
    public GetAllCoordinatesHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetAllCoordinatesResponse> Handle(GetAllCoordinates request, CancellationToken cancellationToken)
    {
        var coordinates = await _context.Landmarks
            .Where(l => l.Latitude != null && l.Longitude != null)
            .Select(l => new CoordinateDto(l.Latitude, l.Longitude))
            .ToListAsync(cancellationToken);
        
        return new GetAllCoordinatesResponse(coordinates);
    }
}