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
            .Must(ValidFullName).WithMessage("The full name should be composed of two to four segments, each ranging from 2 to 15 characters in length.");
        
        RuleFor(x => x.Email)    
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email address.");
    }
   
    private static string ApplySimpleCapitalization(string input)
    {
        var words = input.Trim().ToLower().Split(' ');
        for (var i = 0; i < words.Length; i++)
        {
            if (!string.IsNullOrEmpty(words[i]))
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
        }
        return string.Join(" ", words);
    }
    private static bool ValidFullName(string? fullName)
    {
        var parts = fullName?.Split(' ');
        if (parts?.Length < 2 || parts?.Length > 4)
            return false; 
        foreach (var part in parts?.Where(part => part.Length > 0) ?? Array.Empty<string>())
        {
            if (part.Length is < 2 or > 15)
                return false; 
        }
        return true;
    }

    protected override bool PreValidate(ValidationContext<RegisterUserCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;
        
        if (!string.IsNullOrWhiteSpace(command.FullName))
        {
            command.FullName = ApplySimpleCapitalization(command.FullName);
        }
        command.UserName = command.UserName?.Trim();
        command.Password = command.Password?.Trim();
        command.FullName = command.FullName?.Trim();
        command.Email = command.Email?.Trim();

        return true;
    }
}