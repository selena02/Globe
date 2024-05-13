namespace Application.Users.Queries.GetUserFollowing;

public class GetUserFollowingQueryValidator : AbstractValidator<GetUserFollowingQuery>
{
    public GetUserFollowingQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id required");
    }
}