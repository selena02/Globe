namespace Application.Comments.Queries.GetPostComments;

public class GetPostCommentsQueryValidator : AbstractValidator<GetPostCommentsQuery>
{
    public GetPostCommentsQueryValidator()
    {
        RuleFor(x => x.PaginationParameters.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal to 1");

        RuleFor(x => x.PaginationParameters.PageSize)
            .InclusiveBetween(1, 25).WithMessage("Page size must be between 1 and 25");

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Post id required");
    }
}