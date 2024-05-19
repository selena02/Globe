using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        // Properties
        public string FullName { get; set; }
        public string Location { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PicturePublicId { get; set; }
        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Landmark> Landmarks { get; set; }
        public virtual ICollection<Follow> Followers { get; set; }
        public virtual ICollection<Follow> Following { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
