using Application.Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces;

public interface ICloudinaryService
{
    Task<UploadImageResult?> UploadLandmarkImage(FormFile image);
    Task<UploadImageResult?> UploadProfileImage(FormFile image);
    Task<DeleteImageResult> DeleteImageAsync(string publicId);
}