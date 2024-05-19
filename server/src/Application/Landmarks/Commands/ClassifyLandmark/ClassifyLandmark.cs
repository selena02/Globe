using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Landmarks.Commands.ClassifyLandmark;

public class ClassifyLandmarkCommand : ICommand<ClassifyLandmarkResponse>
{
    public IFormFile? LandmarkImage { get; set; }
}

public record ClassifyLandmarkResponse(LandmarkDetectorResponse Landmark, LocationDetailsDto LocationDetails);

public class ClassifyLandmarkCommandHandler : ICommandHandler<ClassifyLandmarkCommand, ClassifyLandmarkResponse>
{
    private readonly IAuthService _authService;
    private readonly ILandmarkService _landmarkService;
    private readonly IGeocodingService _geocodingService;
    
    public ClassifyLandmarkCommandHandler(IAuthService authService, ILandmarkService landmarkService, IGeocodingService geocodingService)
    {
        _authService = authService;
        _landmarkService = landmarkService;
        _geocodingService = geocodingService;
    }
    
    public async Task<ClassifyLandmarkResponse> Handle(ClassifyLandmarkCommand request, CancellationToken cancellationToken)
    {
        var landmarkDetectorResponse = await _landmarkService.GetLandmarkNameAsync(request.LandmarkImage);
        
        var landmarkDetails = await _geocodingService.GetLocationDetailsAsync(landmarkDetectorResponse.Name);

        return new ClassifyLandmarkResponse(landmarkDetectorResponse, landmarkDetails);
    }
}