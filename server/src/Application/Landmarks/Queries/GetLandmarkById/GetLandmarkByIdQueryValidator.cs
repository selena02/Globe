namespace Application.Landmarks.Queries.GetLandmarkById;

public class GetLandmarkByIdQueryValidator : AbstractValidator<GetLandmarkByIdQuery>
{
    public GetLandmarkByIdQueryValidator()
    {
        RuleFor(x => x.LandmarkId)
            .NotEmpty().WithMessage("Landmark ID required");
    }
}