using FluentValidation.Results;

namespace Application.Landmarks.Commands.SaveLandmark;

public class SaveLandmarkCommandValidator : AbstractValidator<SaveLandmarkCommand>
{
    public SaveLandmarkCommandValidator()
    {
        RuleFor(x => x.Review)
            .MaximumLength(500).WithMessage("Review must not exceed 500 characters");
        
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
    }
    
    protected override bool PreValidate(ValidationContext<SaveLandmarkCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;

        command.Review = command.Review?.Trim();

        return true;
    }
}