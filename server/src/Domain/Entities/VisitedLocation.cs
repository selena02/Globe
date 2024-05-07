namespace Domain.Entities;

public class VisitedLocation
{
    // Properties
    public int VisitedLocationId { get; set; }
    public string LocationName { get; set; }
    public DateTime VisitedOn { get; set; } = DateTime.UtcNow;
    public string Description { get; set; }
    public bool IsPrivate { get; set; } = false;
    public string PhotoUrl { get; set; }
    public string PublicId { get; set; }
    public int? Rating { get; init; }
    public string Details { get; init; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation property
    public virtual User User { get; set; }
}