using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Posts.Commands.EditPost;

public class EditPostCommandValidator : AbstractValidator<EditPostCommand>
{
    public EditPostCommandValidator()
    {
        RuleFor(x=> x.PostId)
            .NotEmpty().WithMessage("Post ID is required");
        
        RuleFor(x => x.Content)
            .MaximumLength(500).WithMessage("Content must be under 2000 characters.");

        RuleFor(x => x.PostPicture)
            .Must(BeAPngOrJpeg).WithMessage("Post picture must be a PNG or JPEG file.");
    }
    
    private static bool BeAPngOrJpeg(IFormFile? file)
    {
        if (file is not null)
        {
            return file.ContentType is "image/jpeg" or "image/png";
        }

        return true;
    }

    protected override bool PreValidate(ValidationContext<EditPostCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;

        command.Content = command.Content?.Trim();

        if (string.IsNullOrWhiteSpace(command.Content) && command.PostPicture is null)
        {
            result.Errors.Add(new ValidationFailure("EditPostCommand", "No content provided"));
        }

        return true;
    }
    
}