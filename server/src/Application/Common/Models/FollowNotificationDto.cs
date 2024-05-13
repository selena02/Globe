namespace Application.Common.Models;

public record FollowNotificationDto(
    int NotificationId,
    int FollowerId,
    string FollowerUsername,
    string? FollowerProfilePicture,
    DateTime FollowedAt);
