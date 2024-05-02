namespace Domain.Entities;

public class Like
{
    // Properties
    public int LikeId { get; set; }

    // Foreign keys
    public int UserId { get; set; }
    public int? PostId { get; set; }
    public int? CommentId { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    public virtual Post Post { get; set; }
    public Comment Comment { get; set; }
}