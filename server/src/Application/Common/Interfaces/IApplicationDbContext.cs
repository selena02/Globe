using Microsoft.EntityFrameworkCore.Storage;
using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Post> Posts { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<Like> Likes { get; set; }
    DbSet<Landmark> Landmarks { get; set; }
    DbSet<Follow> Follows { get; set; }
    DbSet<Notification> Notifications { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}