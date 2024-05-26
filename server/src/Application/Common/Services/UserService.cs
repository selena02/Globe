using Application.Common.Interfaces;
using Domain.Exceptions;

namespace Application.Common.Services;

public class UserService : IUserService
{
    private readonly IApplicationDbContext _context;
    
    public UserService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken)
    {
        var transaction = await _context.BeginTransactionAsync(cancellationToken);
        try
        {
            var user = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException($"User with ID {userId} not found");
            }
            
            await DeleteUserPostsAsync(userId, cancellationToken);
            await DeleteUserLandmarksAsync(userId, cancellationToken);
            await DeleteUserNotificationsAsync(userId, cancellationToken);
            await DeleteUserFollowsAsync(userId, cancellationToken);

            _context.Users.Remove(user);
            
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    
    private async Task DeleteUserPostsAsync(int userId, CancellationToken cancellationToken)
    {
        var posts = await _context.Posts
            .Where(p => p.UserId == userId)
            .ToListAsync(cancellationToken);
            
        _context.Posts.RemoveRange(posts);

        var userLikes = await _context
            .Likes
            .Where(l => l.UserId == userId)
            .ToListAsync(cancellationToken);

        foreach (var like in userLikes)
        {
            var post = _context
                .Posts
                .FirstOrDefault(p => p.PostId == like.PostId);
            
            if (post != null)
            {
                post.LikesCount -= 1;
            }

            var comment = _context
                .Comments
                .FirstOrDefault(c => c.CommentId == like.CommentId);
            
            if (comment != null)
            {
                comment.LikesCount -= 1;
            }
        }
    }
    
    private async Task DeleteUserLandmarksAsync(int userId, CancellationToken cancellationToken)
    {
        var landmarks = await _context
            .Landmarks
            .Where(l => l.UserId == userId)
            .ToListAsync(cancellationToken);
        
        _context.Landmarks.RemoveRange(landmarks);
    }
    
    private async Task DeleteUserNotificationsAsync(int userId, CancellationToken cancellationToken)
    {
        var notifications = await _context
            .Notifications
            .Where(n => n.UserId == userId)
            .ToListAsync(cancellationToken);
        
        _context.Notifications.RemoveRange(notifications);
    }
    
    private async Task DeleteUserFollowsAsync(int userId, CancellationToken cancellationToken)
    {
        var followers = await _context
            .Follows
            .Where(f => f.FollowingId == userId)
            .ToListAsync(cancellationToken);
        
        var followings = await _context
            .Follows
            .Where(f => f.FollowerId == userId)
            .ToListAsync(cancellationToken);
        
        _context.Follows.RemoveRange(followers);
        _context.Follows.RemoveRange(followings);
    }
}