namespace Application.Likes.Queries.GetPostLikes;

public class GetPostLikesQueryValidator : AbstractValidator<GetPostLikesQuery>
{
    public GetPostLikesQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage("Post id required");
    }
}