using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface ICloudinaryService
{
    Task<UploadImageResult?> UploadLandmarkImage(IFormFile image);
    Task<UploadImageResult?> UploadProfileImage(IFormFile image);
    Task<DeleteImageResult> DeleteImageAsync(string publicId);
}