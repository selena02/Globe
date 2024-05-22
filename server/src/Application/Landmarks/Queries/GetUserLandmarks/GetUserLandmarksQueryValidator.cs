namespace Application.Landmarks.Queries.GetUserLandmarks;

public class GetUserLandmarksQueryValidator : AbstractValidator<GetUserLandmarksQuery>
{
    public GetUserLandmarksQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
    }
}