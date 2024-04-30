using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Common.Interfaces
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
    }
}