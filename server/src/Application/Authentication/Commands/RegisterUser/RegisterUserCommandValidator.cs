using Application.Common.Utils;
using FluentValidation.Results;

namespace Application.Authentication.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username required.")
            .Length(5, 15).WithMessage("Username must be between 5 and 15 characters.")
            .Matches(@"^[a-zA-Z0-9_]*$").WithMessage("Username must include only alphanumeric characters and underscores");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(20).WithMessage("Password length must be under 20 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .Must(ValidationUtils.IsValidFullName).WithMessage("The full name should be composed of two to four segments, each ranging from 2 to 15 characters in length.");
        
        RuleFor(x => x.Email)    
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
    }

    protected override bool PreValidate(ValidationContext<RegisterUserCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;
        
        if (!string.IsNullOrWhiteSpace(command.FullName))
        {
            command.FullName = ValidationUtils.ApplySimpleCapitalization(command.FullName);
        }
        command.UserName = command.UserName?.Trim();
        command.Password = command.Password?.Trim();
        command.FullName = command.FullName?.Trim();
        command.Email = command.Email?.Trim();

        return true;
    }
}