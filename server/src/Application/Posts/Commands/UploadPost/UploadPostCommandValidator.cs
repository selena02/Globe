using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands.UploadPost;

public class UploadPostCommandValidator : AbstractValidator<UploadPostCommand>
{
    public UploadPostCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(500).WithMessage("Content must be under 500 characters");

        RuleFor(x => x.PostImage)
            .NotNull().WithMessage("Post image is required")
            .Must(BeAJpegOrPng).WithMessage("Post image must be a JPEG or PNG file");
    }
    private static bool BeAJpegOrPng(IFormFile? file)
    {
        if (file is not null)
        {
            return file.ContentType is "image/jpeg" or "image/png";
        }
        
        return true;
    }

    protected override bool PreValidate(ValidationContext<UploadPostCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;

        command.Content = command.Content?.Trim();

        return true;
    }
}