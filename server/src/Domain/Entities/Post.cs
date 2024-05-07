namespace Domain.Entities;

public class Post
{
    // Properties
    public int PostId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string PhotoUrl { get; set; }
    public string PublicId { get; set; }
    public int LikesCount { get; set; } = 0;
    public int CommentsCount { get; set; } = 0;

    // Foreign keys
    public int UserId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Like> Likes { get; set; }
}