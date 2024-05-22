namespace Domain.Entities;

public class Landmark
{
    // Properties
    public int LandmarkId { get; set; }
    public string LocationName { get; set; }
    public DateTime VisitedOn { get; init; } = DateTime.UtcNow;
    public string Review { get; set; }
    public string PhotoUrl { get; set; }
    public string PublicId { get; set; }
    public int Rating { get; init; } = 1;
    public string Longitude { get; set; }
    public string Latitude { get; set; }
    public string Country { get; set; }
    public string City { get; set; }

    // Foreign key
    public int UserId { get; set; }

    // Navigation property
    public virtual User User { get; set; }
}