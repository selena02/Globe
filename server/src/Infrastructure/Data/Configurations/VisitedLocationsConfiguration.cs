using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VisitedLocationsConfiguration : IEntityTypeConfiguration<VisitedLocation>
{
    public void Configure(EntityTypeBuilder<VisitedLocation> entity)
    {
        entity.HasKey(vl => vl.VisitedLocationId);
        
        entity.HasOne(vl => vl.User)
            .WithMany(u => u.VisitedLocations)
            .HasForeignKey(vl => vl.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // This makes the rating be 1 - 5
        entity.Property(vl => vl.Rating)
            .HasDefaultValue(null) 
            .HasMaxLength(5)
            .IsFixedLength();
    }
}