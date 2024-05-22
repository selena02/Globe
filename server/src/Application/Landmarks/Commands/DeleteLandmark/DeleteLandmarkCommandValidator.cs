namespace Application.Landmarks.Commands.DeleteLandmark;

public class DeleteLandmarkCommandValidator : AbstractValidator<DeleteLandmarkCommand>
{
    public DeleteLandmarkCommandValidator()
    {
        RuleFor(x => x.LandmarkId)
            .NotEmpty().WithMessage("Landmark ID required");
    }
}