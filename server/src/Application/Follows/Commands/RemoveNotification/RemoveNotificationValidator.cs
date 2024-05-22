namespace Application.Follows.Commands.RemoveNotification;

public class RemoveNotificationValidator : AbstractValidator<RemoveNotificationCommand>
{
    public RemoveNotificationValidator()
    {
        RuleFor(x => x.NotificationId)
            .NotEmpty().WithMessage("NotificationId is required");
    }
}