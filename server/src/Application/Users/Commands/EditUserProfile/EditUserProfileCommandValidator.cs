using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.EditUserProfile;

public class EditUserProfileCommandValidator : AbstractValidator<EditUserProfileCommand>
{
    public EditUserProfileCommandValidator()
    {
        RuleFor(x => x.EditProfileDto)
            .NotEmpty().WithMessage("Content required");

        RuleFor(x => x.EditProfileDto.Bio)
            .MaximumLength(200).WithMessage("Bio must be under 200 characters.");

        RuleFor(x => x.EditProfileDto.Location)
            .MaximumLength(50).WithMessage("Location must be under 100 characters.");

        RuleFor(x => x.EditProfileDto.ProfilePicture)
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
        
        command.EditProfileDto.Bio = command.EditProfileDto.Bio?.Trim();
        command.EditProfileDto.Location = command.EditProfileDto.Location?.Trim();

        if (string.IsNullOrWhiteSpace(command.EditProfileDto.Bio) && string.IsNullOrWhiteSpace(command.EditProfileDto.Location) && command.EditProfileDto.ProfilePicture is null)
        {
            result.Errors.Add(new ValidationFailure("EditProfileDto", "No content provided"));
        }
    
        return true;
    }
}

