using FluentValidation.Results;

namespace Application.Authentication.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.LoginDto)
            .NotEmpty().WithMessage("Content required");

        RuleFor(x => x.LoginDto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.LoginDto.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}