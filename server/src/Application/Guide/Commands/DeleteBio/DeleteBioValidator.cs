namespace Application.Guide.Commands.DeleteBio;

public class DeleteBioCommandValidator : AbstractValidator<DeleteBioCommand>
{
    public DeleteBioCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
    }
}