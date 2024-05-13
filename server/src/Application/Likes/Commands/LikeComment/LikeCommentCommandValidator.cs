namespace Application.Likes.Commands.LikeComment;

public class LikeCommentCommandValidator : AbstractValidator<LikeCommentCommand>
{
    public LikeCommentCommandValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("Comment ID required");
    }
}