using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Landmarks.Commands.ClassifyLandmark;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Landmarks.Commands.SaveLandmark;

public class SaveLandmarkCommand : ICommand<SaveLandmarkResponse>
{
    public string? Review { get; set; }
    public int Rating { get; set; }
}

public record SaveLandmarkResponse(bool IsSaved);

public class SaveLandmarkCommandHandler : ICommandHandler<SaveLandmarkCommand, SaveLandmarkResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IAuthService _authService;
    private readonly IMemoryCache _memoryCache;
    
    public SaveLandmarkCommandHandler(IApplicationDbContext context, IAuthService authService, IMemoryCache memoryCache)
    {
        _context = context;
        _authService = authService;
        _memoryCache = memoryCache;
    }
    
    public async Task<SaveLandmarkResponse> Handle(SaveLandmarkCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _authService.GetCurrentUserId();
        var cacheKey = $"ClassifyLandmark:{currentUserId}";
        
        if (!_memoryCache.TryGetValue(cacheKey, out ClassifyLandmarkResponse? classifyLandmarkResponse))
        {
            throw new NotFoundException("Landmark not found");
        }
        
        var landmark = new Landmark
        {
            LocationName = classifyLandmarkResponse?.Landmark.Name,
            Latitude = classifyLandmarkResponse?.LocationDetails?.Latitude?.ToString(),
            Longitude = classifyLandmarkResponse?.LocationDetails?.Longitude?.ToString(),
            PublicId = classifyLandmarkResponse?.PhotoId,
            Country = classifyLandmarkResponse?.LocationDetails?.Country,
            City = classifyLandmarkResponse?.LocationDetails?.City,
            Review = request.Review,
            Rating = request.Rating,
            UserId = currentUserId,
            CountryCode = classifyLandmarkResponse?.LocationDetails?.CountryCode
        };
        
        await _context.Landmarks.AddAsync(landmark, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        _memoryCache.Remove(cacheKey);

        return new SaveLandmarkResponse(true);
    }
}