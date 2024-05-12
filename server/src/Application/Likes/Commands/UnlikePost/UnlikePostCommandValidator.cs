namespace Application.Likes.Commands.UnlikePost;

public class UnlikePostCommandValidator : AbstractValidator<UnlikePostCommand>
{
    public UnlikePostCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();
    }
}