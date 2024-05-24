using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Landmarks.Commands.ClassifyLandmark;

public class ClassifyLandmarkCommandValidator : AbstractValidator<ClassifyLandmarkCommand>
{
    public ClassifyLandmarkCommandValidator()
    {
        RuleFor(x => x.LandmarkImage)
            .NotNull().WithMessage("Image is required")
            .Must(BeAJpegOrPng).WithMessage("Image must be a JPEG or PNG file");
    }
    private static bool BeAJpegOrPng(IFormFile? file)
    {
        if (file is not null)
        {
            return file.ContentType is "image/jpeg" or "image/png";
        }
        
        return true;
    }
}