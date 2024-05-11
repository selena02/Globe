namespace Application.Common.Models;

public record PostDto (
    int UserId,
    string UserName,
    string? ProfilePictureUrl,
    int LikesCount,
    int CommentsCount,
    int PostId,
    string PostPictureUrl
);