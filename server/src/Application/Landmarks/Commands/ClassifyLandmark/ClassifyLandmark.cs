using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Landmarks.Commands.ClassifyLandmark;

public class ClassifyLandmarkCommand : ICommand<ClassifyLandmarkResponse>
{
    public IFormFile? LandmarkImage { get; set; }
}

public record ClassifyLandmarkResponse(
    LandmarkDetectorResponse Landmark, 
    LocationDetailsDto LocationDetails,
    string? PhotoId,
    bool? CanSave
    );

public class ClassifyLandmarkCommandHandler : ICommandHandler<ClassifyLandmarkCommand, ClassifyLandmarkResponse>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IAuthService _authService;
    private readonly ILandmarkService _landmarkService;
    private readonly IGeocodingService _geocodingService;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IApplicationDbContext _context;
    
    public ClassifyLandmarkCommandHandler(IMemoryCache memoryCache,IAuthService authService, 
        ILandmarkService landmarkService, IGeocodingService geocodingService, ICloudinaryService cloudinaryService, IApplicationDbContext context)
    {
        _memoryCache = memoryCache;
        _authService = authService;
        _landmarkService = landmarkService;
        _geocodingService = geocodingService;
        _cloudinaryService = cloudinaryService;
        _context = context;
    }
    
    public async Task<ClassifyLandmarkResponse> Handle(ClassifyLandmarkCommand request, CancellationToken cancellationToken)
    {
        var landmarkDetectorResponse = await _landmarkService.GetLandmarkNameAsync(request.LandmarkImage);
        
        var currrentUserId = _authService.GetCurrentUserId();
        
        var currentUser = await _context.Users
            .Include(u => u.Landmarks)
            .FirstOrDefaultAsync(u => u.Id == currrentUserId, cancellationToken);
        
        if (currentUser is null)
        {
            throw new NotFoundException("User not found");
        }

        var alreadySaved = currentUser?.Landmarks.Any(l => l.LocationName == landmarkDetectorResponse.Name) ?? false;

        if (alreadySaved)
        {
            var landmark = currentUser.Landmarks.FirstOrDefault(l => l.LocationName == landmarkDetectorResponse.Name);
            
            return new ClassifyLandmarkResponse(
                landmarkDetectorResponse, 
                new LocationDetailsDto
                {
                    Latitude = landmark?.Latitude != null ? double.Parse(landmark.Latitude) : (double?)null,
                    Longitude = landmark?.Longitude != null ? double.Parse(landmark.Longitude) : (double?)null,
                    Country = landmark?.Country,
                    City = landmark?.City,
                    Errors = null
                },
                landmark?.PublicId, 
                true);
        }
        
        var landmarkDetails = await _geocodingService.GetLocationDetailsAsync(landmarkDetectorResponse.Name);
        
        var photoUploadResult = await _cloudinaryService.UploadLandmarkImage(request.LandmarkImage);
        
        if (photoUploadResult?.Errors is not null)
        {
            throw new ServerErrorException("Error uploading picture to cloudinary");
        }

        var cacheKey = $"ClassifyLandmark:{currrentUserId}";
        _memoryCache.Set(cacheKey, new ClassifyLandmarkResponse(
            landmarkDetectorResponse, 
            landmarkDetails, 
            photoUploadResult?.PublicId, 
            null), TimeSpan.FromMinutes(30));
        
        return new ClassifyLandmarkResponse(landmarkDetectorResponse, landmarkDetails, photoUploadResult?.PublicId, alreadySaved);
    }
}