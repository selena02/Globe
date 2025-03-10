﻿using Application.Common.Abstractions;
using Application.Common.Interfaces;

namespace Application.Landmarks.Queries.GetAllCoordinates;

public record GetAllCoordinatesQuery() : IQuery<GetAllCoordinatesResponse>;

public record GetAllCoordinatesResponse(List<CoordinateDto> Coordinates);

public record CoordinateDto(string Latitude, string Longitude, string LocationName); 

public class GetAllCoordinatesHandler : IQueryHandler<GetAllCoordinatesQuery, GetAllCoordinatesResponse>
{
    private readonly IApplicationDbContext _context;
    
    public GetAllCoordinatesHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetAllCoordinatesResponse> Handle(GetAllCoordinatesQuery request, CancellationToken cancellationToken)
    {
        var coordinates = await _context.Landmarks
            .Where(l => l.Latitude != null && l.Longitude != null)
            .Select(l => new CoordinateDto(l.Latitude, l.Longitude, l.LocationName))
            .ToListAsync(cancellationToken);
        
        var hashSet = new HashSet<CoordinateDto>(coordinates);
        
        foreach (var coordinate in coordinates)
        {
            hashSet.Add(coordinate);
        }
        
        coordinates = hashSet.ToList();
        
        return new GetAllCoordinatesResponse(coordinates);
    }
}