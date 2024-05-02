namespace Domain.Entities;

public class Follow
{
    // Properties
    public DateTime FollowedDate { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }

    // Navigation property
    public virtual User Follower { get; set; }
    public virtual User Following { get; set; }
}