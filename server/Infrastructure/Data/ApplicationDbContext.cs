using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<VisitedLocation> VisitedLocation { get; set; }
    public DbSet<Follow> Follow { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Posts
        modelBuilder.ApplyConfiguration(new PostsConfiguration());

        // Comments
        modelBuilder.ApplyConfiguration(new CommentsConfiguration());

        // Likes
        modelBuilder.ApplyConfiguration(new LikesConfiguration());
        
        // VisitedLocations
        modelBuilder.ApplyConfiguration(new VisitedLocationsConfiguration());

        // Followers
        modelBuilder.ApplyConfiguration(new FollowersConfiguration());
        
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await this.Database.BeginTransactionAsync(cancellationToken);
    }
} 
