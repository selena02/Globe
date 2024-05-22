namespace Application.Guide.Commands;

public class DeletePictureCommandValidator : AbstractValidator<DeletePictureCommand>
{
    public DeletePictureCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
    }
}