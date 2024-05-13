namespace Application.Users.Queries.GetUserFollowers;

public class GetUserFollowersQueryValidator : AbstractValidator<GetUserFollowersQuery>
{
    public GetUserFollowersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id required");
    }
}