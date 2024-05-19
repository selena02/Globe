using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface ILandmarkService
{
    Task<LandmarkDetectorResponse> GetLandmarkNameAsync(IFormFile? file);
}