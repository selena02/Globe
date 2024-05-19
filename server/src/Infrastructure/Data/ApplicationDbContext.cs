using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions options)
    : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options), IApplicationDbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Landmark> Landmarks { get; set; }
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Users
        modelBuilder.ApplyConfiguration(new UsersConfiguration());

        // Roles
        modelBuilder.ApplyConfiguration(new RolesConfiguration());
        
        // Posts
        modelBuilder.ApplyConfiguration(new PostsConfiguration());

        // Comments
        modelBuilder.ApplyConfiguration(new CommentsConfiguration());

        // Likes
        modelBuilder.ApplyConfiguration(new LikesConfiguration());
        
        // VisitedLocations
        modelBuilder.ApplyConfiguration(new LandmarksConfiguration());

        // Followers
        modelBuilder.ApplyConfiguration(new FollowersConfiguration());
        
        // Notifications
        modelBuilder.ApplyConfiguration(new NotificationsConfiguration());
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await this.Database.BeginTransactionAsync(cancellationToken);
    }
} 
