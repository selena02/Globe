using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; init; }
    }
}
