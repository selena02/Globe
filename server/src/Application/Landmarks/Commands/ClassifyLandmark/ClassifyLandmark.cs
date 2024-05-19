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
    string? PhotoId
    );

public class ClassifyLandmarkCommandHandler : ICommandHandler<ClassifyLandmarkCommand, ClassifyLandmarkResponse>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IAuthService _authService;
    private readonly ILandmarkService _landmarkService;
    private readonly IGeocodingService _geocodingService;
    private readonly ICloudinaryService _cloudinaryService;
    
    public ClassifyLandmarkCommandHandler(IMemoryCache memoryCache,IAuthService authService, 
        ILandmarkService landmarkService, IGeocodingService geocodingService, ICloudinaryService cloudinaryService)
    {
        _memoryCache = memoryCache;
        _authService = authService;
        _landmarkService = landmarkService;
        _geocodingService = geocodingService;
        _cloudinaryService = cloudinaryService;
    }
    
    public async Task<ClassifyLandmarkResponse> Handle(ClassifyLandmarkCommand request, CancellationToken cancellationToken)
    {
        var landmarkDetectorResponse = await _landmarkService.GetLandmarkNameAsync(request.LandmarkImage);
        
        var landmarkDetails = await _geocodingService.GetLocationDetailsAsync(landmarkDetectorResponse.Name);
        
        var photoUploadResult = await _cloudinaryService.UploadLandmarkImage(request.LandmarkImage);
        
        if (photoUploadResult?.Errors is not null)
        {
            throw new ServerErrorException("Error uploading picture to cloudinary");
        }
        
        var cacheKey = $"ClassifyLandmark:{_authService.GetCurrentUserId()}";
        
        _memoryCache.Set(cacheKey, new ClassifyLandmarkResponse(landmarkDetectorResponse, landmarkDetails, photoUploadResult?.PublicId), TimeSpan.FromMinutes(30));

        return new ClassifyLandmarkResponse(landmarkDetectorResponse, landmarkDetails, photoUploadResult?.PublicId);
    }
}