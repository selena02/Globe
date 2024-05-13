namespace Application.Common.Models;

public record CommentDto(
    int CommentId,
    int UserId,
    string UserName,
    string? ProfilePicture,
    string Content,
    int LikesCount,
    DateTime CreatedAt,
    bool IsLikedByCurrentUser,
    bool canDelete
);