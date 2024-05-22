using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.EditUserProfile;

public class EditUserProfileCommandValidator : AbstractValidator<EditUserProfileCommand>
{
    public EditUserProfileCommandValidator()
    {
        RuleFor(x => x.Bio)
            .MaximumLength(200).WithMessage("Bio must be under 200 characters.");

        RuleFor(x => x.ProfilePicture)
            .Must(BeAPngOrJpeg).WithMessage("Profile picture must be a PNG or JPEG file.");
    }
    private static bool BeAPngOrJpeg(IFormFile? file)
    {
        if (file is not null)
        {
            return file.ContentType is "image/jpeg" or "image/png";
        }
        
        return true; 
    }
    protected override bool PreValidate(ValidationContext<EditUserProfileCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;
        
        command.Bio = command.Bio?.Trim();
        
        if (string.IsNullOrWhiteSpace(command.Bio) && command.ProfilePicture is null)
        {
            result.Errors.Add(new ValidationFailure("EditProfileCommand", "No content provided"));
        }
    
        return true;
    }
}

