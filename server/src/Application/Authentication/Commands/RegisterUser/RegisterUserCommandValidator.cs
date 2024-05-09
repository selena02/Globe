using FluentValidation.Results;

namespace Application.Authentication.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.RegisterDto)
            .NotEmpty().WithMessage("Content required");
        
        RuleFor(x => x.RegisterDto.UserName)
            .NotEmpty().WithMessage("Username required.")
            .MinimumLength(4).WithMessage("Username must contain at least 4 characters.")
            .MaximumLength(10).WithMessage("Username cannot exceed 10 characters.")
            .Matches(@"^[a-zA-Z\s]*$").WithMessage("Username must include standard characters");
    }
    
    protected override bool PreValidate(ValidationContext<RegisterUserCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;

        /*command.Username = command.Username?.ToLower().Trim();
        command.Password = command.Password?.Trim();
        command.Gender = command.Gender?.ToLower().Trim();
        command.FullName = command.FullName?.Trim();
        command.Country = command.Country?.Trim();*/

        return true;
    }
}