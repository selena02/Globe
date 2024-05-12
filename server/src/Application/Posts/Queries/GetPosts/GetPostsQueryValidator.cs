namespace Application.Posts.Queries.GetPosts;

public class GetPostsQueryValidator : AbstractValidator<GetPostsQuery>
{
    public GetPostsQueryValidator()
    {
        RuleFor(x => x.PaginationParameters)
            .NotNull().WithMessage("Pagination parameters required");
        
        RuleFor(x => x.PaginationParameters)
            .NotNull().WithMessage("Pagination parameters required");
        
        RuleFor(x => x.PaginationParameters.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal to 1");

        RuleFor(x => x.PaginationParameters.PageSize)
            .InclusiveBetween(1, 25).WithMessage("Page size must be between 1 and 25");
    }
}