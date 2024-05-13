namespace Domain.Entities;

public class Notification
{
    // Properties
    public int NotificationId { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public int RecieverId { get; set; } 
    
    // Foreign keys
    public int UserId { get; set; }

    // Navigation properties
    public virtual User User { get; set; }
}