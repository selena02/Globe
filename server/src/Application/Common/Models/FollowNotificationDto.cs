namespace Application.Common.Models;

public record FollowNotificationDto(
    int FollowerId,
    string FollowerUsername,
    string? FollowerProfilePicture,
    DateTime FollowedAt);
