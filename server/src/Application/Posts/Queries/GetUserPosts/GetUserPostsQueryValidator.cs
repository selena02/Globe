namespace Application.Posts.Queries.GetUserPosts;

public class GetUserPostsQueryValidator : AbstractValidator<GetUserPostsQuery>
{
    public GetUserPostsQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
        
        RuleFor(x => x.PaginationParameters)
            .NotNull().WithMessage("Pagination parameters required");
        
        RuleFor(x => x.PaginationParameters.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal to 1");

        RuleFor(x => x.PaginationParameters.PageSize)
            .InclusiveBetween(1, 25).WithMessage("Page size must be between 1 and 25");
    }
}