namespace Application.Comments.Queries.GetCommentLikes;

public class GetCommentLikesQueryValidator : AbstractValidator<GetCommentLikesQuery>
{
    public GetCommentLikesQueryValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty().WithMessage("Comment id required");
    }
}