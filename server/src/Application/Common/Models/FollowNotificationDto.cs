namespace Application.Common.Models;

public record FollowNotificationDto(
    int FollowerId,
    string FollowerUsername,
    string? FollowerProfilePictureUrl,
    DateTime FollowedAt);
