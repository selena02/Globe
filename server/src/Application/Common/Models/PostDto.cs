namespace Application.Common.Models;

public record PostDto (
    int UserId,
    string UserName,
    string? ProfilePicture,
    int LikesCount,
    int CommentsCount,
    int PostId,
    string PostPicture
);