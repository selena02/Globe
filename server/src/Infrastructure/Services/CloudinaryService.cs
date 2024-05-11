using Application.Common.Interfaces;
using Application.Common.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        private async Task<UploadImageResult?> UploadImage(IFormFile image, ImageType photoType)
        {
            if (string.IsNullOrEmpty(_cloudinary.Api.Account.Cloud) ||
                string.IsNullOrEmpty(_cloudinary.Api.Account.ApiKey) ||
                string.IsNullOrEmpty(_cloudinary.Api.Account.ApiSecret))
            {
                throw new BadRequestException("Cloudinary account not configured");
            }

            if (image.Length <= 0)
            {
                return null;
            }
            
            await using var stream = image.OpenReadStream();
            Transformation transformation;
            
            switch (photoType)
            {
                case ImageType.Profile:
                    transformation = new Transformation()
                        .Height(400)
                        .Width(400)
                        .Crop("fill")
                        .Gravity("face")
                        .Quality("auto:best")
                        .FetchFormat("auto");
                    break;
                case ImageType.Landmark:
                    transformation = new Transformation()
                        .Width(600) 
                        .Crop("scale") // To maintain original proportions
                        .Quality("auto:best")
                        .FetchFormat("auto");
                    break;
                default:
                    throw new ArgumentException("Invalid photo type specified.");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
                Transformation = transformation,
                Folder = "globe-images"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            
            return new UploadImageResult(uploadResult.PublicId, uploadResult.SecureUrl?.ToString(), uploadResult.Error?.Message);

        }
        public async Task<UploadImageResult?> UploadProfileImage(IFormFile image)
        {
            return await UploadImage(image, ImageType.Profile);
        }
        public async Task<UploadImageResult?> UploadLandmarkImage(IFormFile image)
        {
            return await UploadImage(image, ImageType.Landmark);
        }

        public async Task<DeleteImageResult> DeleteImageAsync(string publicId)
        {
            var deletionResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            return new DeleteImageResult
            (
                deletionResult.Result == "ok",
                deletionResult.Error?.Message
            );
        }
    }
}
