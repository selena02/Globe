namespace Domain.Entities;

public class Comment
{
    // Properties
    public int CommentId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int LikesCount { get; set; } = 0;

    // Foreign keys
    public int UserId { get; set; }
    public int PostId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual Post Post { get; set; }
}