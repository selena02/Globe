using FluentValidation.Results;

namespace Application.Comments.Commands.UploadComment;

public class UploadCommentCommandValidator: AbstractValidator<UploadCommentCommand>
{
    public UploadCommentCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(500).WithMessage("Content must be under 500 characters");
        
        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage("Post ID is required");
    }
    
    protected override bool PreValidate(ValidationContext<UploadCommentCommand> context, ValidationResult result)
    {
        var command = context.InstanceToValidate;

        command.Content = command.Content?.Trim();

        return true;
    }
    
}