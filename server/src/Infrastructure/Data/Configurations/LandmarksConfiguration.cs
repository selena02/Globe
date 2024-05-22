using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class LandmarksConfiguration : IEntityTypeConfiguration<Landmark>
{
    public void Configure(EntityTypeBuilder<Landmark> entity)
    {
        entity.HasKey(vl => vl.LandmarkId);
        
        entity.HasOne(vl => vl.User)
            .WithMany(u => u.Landmarks)
            .HasForeignKey(vl => vl.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // This makes the rating be 1 - 5
        entity.Property(vl => vl.Rating)
            .HasDefaultValue(null) 
            .HasMaxLength(5)
            .IsFixedLength();
    }
}